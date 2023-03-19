using FileToSql.Infrastructure.Entities;

namespace FileToSql.Infrastructure.Repositories;

public class ListPriceRepository
{
    private readonly FileManagerDbContext _dbContext;

    public ListPriceRepository(FileManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(ListPrice entity)
    {
        await _dbContext.AddAsync(entity);
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task BulkSaveAsync(IEnumerable<ListPrice> listPrices)
    {
        await _dbContext.BulkInsertAsync(listPrices, o =>
        {
            o.AutoMapOutputDirection = false;
        });
    }
}
