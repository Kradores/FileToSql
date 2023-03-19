namespace FileToSql.Infrastructure.Entities;

public class ListPrice
{
    public int Id { get; set; }
    public string PartNumber { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string LongDesc { get; set; } = null!;
    public double FleetPrice { get; set; }
    public string ProductType { get; set; } = null!;
    public double Weight { get; set; }
}
