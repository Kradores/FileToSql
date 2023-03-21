using FileToSql.Infrastructure.Enums;

namespace FileToSql.Infrastructure.Entities;

public class FusedPrice
{
    public string PartNumber { get; set; } = null!;
    public FileContentType ContentType { get; set; }
    public string Description { get; set; } = null!;
    public string Manufacturer { get; set; } = null!;
    public double ListPrice { get; set; }
    public string ProductType { get; set; } = null!;
}
