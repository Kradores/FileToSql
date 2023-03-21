using FileToSql.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

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

    public async Task BulkInsertAsync(IEnumerable<ListPrice> listPrices)
    {
        await _dbContext.BulkInsertAsync(listPrices, o =>
        {
            o.AutoMapOutputDirection = false;
        });
    }

    public async Task<IEnumerable<ListPrice>> GetListPrices(int pageNumber, int pageSize, string search)
    {
        return await _dbContext.ListPrices
                .OrderBy(x => x.PartNumber)
                .Where(x => x.PartNumber.Contains(search))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
    }
}
