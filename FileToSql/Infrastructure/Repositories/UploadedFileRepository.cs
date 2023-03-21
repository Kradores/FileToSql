using DocumentFormat.OpenXml.Wordprocessing;
using FileToSql.Infrastructure.Entities;

namespace FileToSql.Infrastructure.Repositories;

public class UploadedFileRepository
{
    public async Task<List<UploadedFile>?> GetFilesByIdsAsync(List<Guid> fileIds)
    {
        return await Task.Run(() => new List<UploadedFile>()
        {
            new()
            {
                Id = Guid.Parse("59697996-2d3c-4fdd-b8ff-08db25382fb9"),
                ContentType = Enums.FileContentType.PartsMaster,
                Name = "PartsMaster2.xlsx",
                Size = 371694,
                UploadedBy = "mladen.tsvetkov@molsondev.com",
                UploadedOn = DateTime.Now,
            },
            new()
            {
                Id = Guid.Parse("14d9bc94-1838-4e35-b905-08db25382fb9"),
                ContentType = Enums.FileContentType.ListPrice,
                Name = "Manufacturer_Kobelco_HasRules_True_Type_EuroPrice.xlsx",
                Size = 303250,
                UploadedBy = "mladen.tsvetkov@molsondev.com",
                UploadedOn = DateTime.Now,
            }
        });
    }
}
