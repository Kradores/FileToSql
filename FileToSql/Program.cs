using ClosedXML.Excel;
using FileToSql.Configurations;
using FileToSql.Infrastructure.Entities;
using FileToSql.Infrastructure.Repositories;
using System.Diagnostics;

namespace FileToSql
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddDbContext(builder.Configuration)
                .AddRepositories();

            var app = builder.Build();
            app.MigrateAllDbContexts();

            app.MapGet("/", () => "Hello World!");
            app.MapPost("/upload/bulk/partsmaster", UploadBulkPartsHandler);
            app.MapPost("/upload/bulk/listprice", UploadBulkListHandler);
            app.MapPost("/upload/bulk/parallel/listprice", UploadBulkParallelListHandler);
            app.MapPost("/upload/bulk/ext/listprice", UploadBulkExListHandler);
            app.MapPost("/upload/bulk/ext/fusion", UploadBulkExFusionHandler);
            app.MapGet("listprice", GetListPrice);

            app.Run();
        }

        private static async Task<IResult> UploadBulkExFusionHandler(
            HttpContext context,
            FusedPriceRepository fusedPriceRepository,
            UploadedFileRepository uploadedFileRepository,
            FactoringRulesRepository factoringRulesRepository,
            ManufacturerRepository manufacturerRepository)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var filesData = await uploadedFileRepository.GetFilesByIdsAsync(new());

            if (filesData == null) return Results.BadRequest();

            var factoringRules = await factoringRulesRepository.GetRuleByFileIdsAsync(filesData.Select(x => x.Id.ToString()).ToList());
            var manufacturer = factoringRules != null ? await manufacturerRepository.GetByIdAsync(Guid.Empty) : null;

            var response = new Response();
            response.Data.Add($"Upload started", stopwatch.Elapsed.TotalSeconds);

            var files = context.Request.Form.Files;

            var listPriceFile = files
                .Where(x => x.FileName == filesData
                    .Where(y => y.ContentType == Infrastructure.Enums.FileContentType.ListPrice).Select(y => y.Name).First()).First();

            var partsMasterFile = files
                .Where(x => x.FileName == filesData
                    .Where(y => y.ContentType == Infrastructure.Enums.FileContentType.PartsMaster).Select(y => y.Name).First()).First();

            await InsertFusedPriceFromListPriceAsync(
                fusedPriceRepository,
                listPriceFile.OpenReadStream(),
                manufacturer,
                factoringRules,
                response,
                stopwatch);

            await UpdateFusedPriceFromPartsMasterAsync(fusedPriceRepository, partsMasterFile.OpenReadStream(), response, stopwatch);

            stopwatch.Stop();

            return Results.Ok(response);
        }

        private static async Task UpdateFusedPriceFromPartsMasterAsync(
            FusedPriceRepository fusedPriceRepository,
            Stream partsMasterStream,
            Response response,
            Stopwatch stopwatch)
        {
            // you can send as a parameter the file id and get file or stream
            using var workbook = new XLWorkbook(partsMasterStream);
            response.Data.Add($"Opened Parts Master", stopwatch.Elapsed.TotalSeconds);

            var ws = workbook.Worksheet(1);
            var rows = ws.RowsUsed().Skip(1);

            await fusedPriceRepository.BulkMergeAsync(rows.Select(row =>
            {
                var cells = row.Cells(1, 11);

                return new FusedPrice()
                {
                    PartNumber = cells.ElementAt(0).Value.GetText(),
                    ContentType = Infrastructure.Enums.FileContentType.PartsMaster,
                    Description = cells.ElementAt(2).Value.GetText(),
                    Manufacturer = cells.ElementAt(6).Value.GetText(),
                    ListPrice = cells.ElementAt(3).Value.GetNumber(),
                    ProductType = cells.ElementAt(9).Value.ToString(),
                };
            }));

            response.Data.Add($"Update/Insert finished", stopwatch.Elapsed.TotalSeconds);
        }

        private static async Task InsertFusedPriceFromListPriceAsync(
            FusedPriceRepository fusedPriceRepository,
            Stream listPriceStream,
            Manufacturer manufacturer,
            FactoringRules rules,
            Response response,
            Stopwatch stopwatch)
        {
            using var workbook = new XLWorkbook(listPriceStream);
            response.Data.Add($"Opened List Price", stopwatch.Elapsed.TotalSeconds);

            var ws = workbook.Worksheet(1);
            var rows = ws.RowsUsed().Skip(1);

            await fusedPriceRepository.BulkInsertAsync(rows.Select(row =>
            {
                var cells = row.Cells(1, 6);

                return new FusedPrice()
                {
                    PartNumber = cells.ElementAt(0).Value.GetText(),
                    ContentType = Infrastructure.Enums.FileContentType.ListPrice,
                    Description = cells.ElementAt(1).Value.GetText(),
                    Manufacturer = manufacturer.Name,
                    ListPrice = cells.ElementAt(3).Value.GetNumber(),
                    ProductType = cells.ElementAt(4).Value.ToString(),
                }.ApplyFactoringRules(rules);
            }));

            response.Data.Add($"Insert finished", stopwatch.Elapsed.TotalSeconds);
        }

        private static async Task<IEnumerable<ListPrice>> GetListPrice(int pageNumber, int pageSize, string search, ListPriceRepository repository)
        {
            return await repository.GetListPrices(pageNumber, pageSize, search);
        }

        private static async Task<IResult> UploadBulkPartsHandler(HttpContext context, PartsMasterRepository repository)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var response = new Response();
            response.Data.Add($"Upload started", stopwatch.Elapsed.TotalSeconds);

            var files = context.Request.Form.Files;

            var counter = 0;

            foreach (var blob in files)
            {
                using var workbook = new XLWorkbook(blob.OpenReadStream());

                response.Data.Add($"Opened workbook {blob.FileName}", stopwatch.Elapsed.TotalSeconds);

                var ws = workbook.Worksheet(1);
                var rows = ws.RowsUsed().Skip(1);

                using var enumerator = rows.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    counter++;
                    var cells = enumerator.Current.Cells(1, 11);

                    PartsMaster entity = new()
                    {
                        PartNumber = cells.ElementAt(0).Value.GetText(),
                        PartSufix = cells.ElementAt(1).Value.GetText(),
                        Description = cells.ElementAt(2).Value.GetText(),
                        ListPrice = cells.ElementAt(3).Value.GetNumber(),
                        PartSalePrice = cells.ElementAt(4).Value.GetNumber(),
                        PartMfrCode = cells.ElementAt(5).Value.GetText(),
                        PartmfrDescription = cells.ElementAt(6).Value.GetText(),
                        PartSource = cells.ElementAt(7).Value.GetText(),
                        PartSourceDescription = cells.ElementAt(8).Value.GetText(),
                        ProductType = cells.ElementAt(9).Value.ToString(),
                        ProductTypeDescription = cells.ElementAt(10).Value.ToString(),
                    };

                    await repository.Add(entity);
                    
                    if (counter % 10000 == 0)
                    {
                        await repository.Save();
                        response.Data.Add($"Insert rows:{counter}", stopwatch.Elapsed.TotalSeconds);
                    }
                }
            }

            await repository.Save();

            response.Data.Add($"Insert finished, rows:{counter}", stopwatch.Elapsed.TotalSeconds);

            stopwatch.Stop();

            return Results.Ok(response);
        }

        private static async Task<IResult> UploadBulkExListHandler(HttpContext context, ListPriceRepository repository)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var response = new Response();
            response.Data.Add($"Upload started", stopwatch.Elapsed.TotalSeconds);

            var files = context.Request.Form.Files;

            foreach (var blob in files)
            {
                using var workbook = new XLWorkbook(blob.OpenReadStream());

                response.Data.Add($"Opened workbook {blob.FileName}", stopwatch.Elapsed.TotalSeconds);

                var ws = workbook.Worksheet(1);
                var rows = ws.RowsUsed().Skip(1);

                await repository.BulkInsertAsync(rows.Select(row =>
                {
                    var cells = row.Cells(1, 6);

                    return new ListPrice()
                    {
                        PartNumber = cells.ElementAt(0).Value.GetText(),
                        Description = cells.ElementAt(1).Value.GetText(),
                        LongDesc = cells.ElementAt(2).Value.GetText(),
                        FleetPrice = cells.ElementAt(3).Value.GetNumber(),
                        ProductType = cells.ElementAt(4).Value.ToString(),
                        Weight = cells.ElementAt(5).Value.GetNumber()
                    };
                }));
            }

            response.Data.Add($"Insert finished", stopwatch.Elapsed.TotalSeconds);
            
            stopwatch.Stop();

            return Results.Ok(response);
        }

        private static async Task<IResult> UploadBulkListHandler(HttpContext context, ListPriceRepository repository)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var response = new Response();
            response.Data.Add($"Upload started", stopwatch.Elapsed.TotalSeconds);

            var files = context.Request.Form.Files;

            var counter = 0;

            foreach (var blob in files)
            {
                using var workbook = new XLWorkbook(blob.OpenReadStream());

                response.Data.Add($"Opened workbook {blob.FileName}", stopwatch.Elapsed.TotalSeconds);

                var ws = workbook.Worksheet(1);
                var rows = ws.RowsUsed().Skip(1);

                using var enumerator = rows.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    counter++;
                    var cells = enumerator.Current.Cells(1, 6);

                    ListPrice entity = new()
                    {
                        PartNumber = cells.ElementAt(0).Value.GetText(),
                        Description = cells.ElementAt(1).Value.GetText(),
                        LongDesc = cells.ElementAt(2).Value.GetText(),
                        FleetPrice = cells.ElementAt(3).Value.GetNumber(),
                        ProductType = cells.ElementAt(4).Value.ToString(),
                        Weight = cells.ElementAt(5).Value.GetNumber()
                    };

                    await repository.Add(entity);

                    if (counter % 4000 == 0)
                    {
                        await repository.Save();
                        response.Data.Add($"Insert rows: {counter}", stopwatch.Elapsed.TotalSeconds);
                    }


                }
            }

            await repository.Save();

            response.Data.Add($"Insert finished, rows:{counter}", stopwatch.Elapsed.TotalSeconds);

            stopwatch.Stop();

            return Results.Ok(response);
        }

        private static async Task<IResult> UploadBulkParallelListHandler(
            HttpContext context,
            IServiceProvider serviceProvider,
            ListPriceRepository repository,
            CancellationToken cancellationToken)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var response = new Response();
            response.Data.Add($"Upload started", stopwatch.Elapsed.TotalSeconds);

            var files = context.Request.Form.Files;

            foreach (var blob in files)
            {
                using var workbook = new XLWorkbook(blob.OpenReadStream());

                response.Data.Add($"Opened workbook {blob.FileName}", stopwatch.Elapsed.TotalSeconds);

                var ws = workbook.Worksheet(1);
                var rows = ws.RowsUsed().Skip(1);

                var batchSize = 10_000;
                var batchCount = 0;
                var rowsCount = rows.Count();

                List<IEnumerable<IXLRow>> batches = new();

                while (rowsCount > 0)
                {
                    batches.Add(rows.Skip(batchCount * batchSize).Take(batchSize));
                    rowsCount -= batchSize;
                    batchCount++;
                }

                var tasks = batches.Select(batch => Task.Run(() => InsertBatch(batch, serviceProvider)));

                await Task.WhenAll(tasks);
            }

            stopwatch.Stop();
            response.Data.Add($"Insert finished", stopwatch.Elapsed.TotalSeconds);

            return Results.Ok(response);
        }

        private static async Task InsertBatch(IEnumerable<IXLRow> rows, IServiceProvider serviceProvider)
        {
            var counter = 0;
            using var scope = serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<ListPriceRepository>();

            using var enumerator = rows.GetEnumerator();

            while (enumerator.MoveNext())
            {
                counter++;
                var cells = enumerator.Current.Cells(1, 6);

                ListPrice entity = new()
                {
                    PartNumber = cells.ElementAt(0).Value.GetText(),
                    Description = cells.ElementAt(1).Value.GetText(),
                    LongDesc = cells.ElementAt(2).Value.GetText(),
                    FleetPrice = cells.ElementAt(3).Value.GetNumber(),
                    ProductType = cells.ElementAt(4).Value.ToString(),
                    Weight = cells.ElementAt(5).Value.GetNumber()
                };

                await repository.Add(entity);

                if (counter % 2000 == 0)
                {
                    await repository.Save();
                }
            }

            await repository.Save();
        }

        public class Response
        {
            public IDictionary<string, double> Data { get; set; } = new Dictionary<string, double>();
        }
    }
}