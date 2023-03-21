using FileToSql.Infrastructure.Entities;

namespace FileToSql.Infrastructure.Repositories;

public class ManufacturerRepository
{
    public async Task<Manufacturer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => new Manufacturer()
        {
            Id = Guid.Parse("30e01be6-eabf-4ded-74b6-08da3caf4ddf"),
            Name = "Kobelco"
        });
    }
}
