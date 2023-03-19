using FileToSql.Infrastructure.Entities;

namespace FileToSql.Infrastructure.Repositories;

public class PartsMasterRepository
{
    private readonly FileManagerDbContext _dbContext;

    public PartsMasterRepository(FileManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAndSave(PartsMaster entity)
    {
        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Add(PartsMaster entity)
    {
        await _dbContext.AddAsync(entity);
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}
