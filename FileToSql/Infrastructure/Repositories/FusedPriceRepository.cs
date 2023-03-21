using FileToSql.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FileToSql.Infrastructure.Repositories;

public class FusedPriceRepository
{
    private readonly FileManagerDbContext _dbContext;

    public FusedPriceRepository(FileManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task BulkInsertAsync(IEnumerable<FusedPrice> fusedPrices)
    {
        await _dbContext.BulkInsertAsync(fusedPrices, o =>
        {
            o.AutoMapOutputDirection = false;
        });
    }

    public async Task BulkMergeAsync(IEnumerable<FusedPrice> fusedPrices)
    {
        await _dbContext.BulkMergeAsync(fusedPrices, o =>
        {
            o.AutoMapOutputDirection = false;
        });
    }

    public async Task BulkUpdateAsync(IEnumerable<FusedPrice> fusedPrices)
    {
        await _dbContext.BulkUpdateAsync(fusedPrices, o =>
        {
            o.AutoMapOutputDirection = false;
        });
    }

    public async Task<IEnumerable<FusedPrice>> GetListPrices(int pageNumber, int pageSize, string search)
    {
        return await _dbContext.FusedPrices
                .OrderBy(x => x.PartNumber)
                .Where(x => x.PartNumber.Contains(search))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
    }
}
