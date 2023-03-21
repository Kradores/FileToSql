namespace FileToSql.Infrastructure.Entities;

public class Manufacturer
{
    public Manufacturer() { }
    public Manufacturer(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
}
