using FileToSql.Infrastructure.Enums;

namespace FileToSql.Infrastructure.Entities;

public class FactoringRules
{
    public FactoringRules() { }
    public FactoringRules(Guid id, List<string> fileIds, Guid manufacturerId, List<FactoringRule> rules)
    {
        Id = id;
        FileIds = fileIds;
        ManufacturerId = manufacturerId;
        Rules = rules;
    }

    public Guid Id { get; init; }
    public List<string> FileIds { get; init; } = new List<string>();
    public Guid ManufacturerId { get; init; }
    public List<FactoringRule> Rules { get; set; } = new List<FactoringRule>();
    public Currency CurrencyRate { get; set; } = default!;

    public class FactoringRule
    {
        public FactoringOperator Operator { get; init; }
        public double Value { get; init; }
        public double Factoring { get; init; }
    }
    public class Currency
    {
        public double Rate { get; set; }
        public CurrencyType CurrencyFrom { get; set; }
        public CurrencyType CurrencyTo { get; set; }
    }
}
