namespace FileToSql.Infrastructure.Entities;

public class PartsMaster
{
    public int Id { get; set; }
    public string PartNumber { get; set; } = null!;
    public string PartSufix { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double ListPrice { get; set; }
    public double PartSalePrice { get; set; }
    public string PartMfrCode { get; set; } = null!;
    public string PartmfrDescription { get; set; } = null!;
    public string PartSource { get; set; } = null!;
    public string PartSourceDescription { get; set; } = null!;
    public string ProductType { get; set; } = null!;
    public string ProductTypeDescription { get; set; } = null!;
}
