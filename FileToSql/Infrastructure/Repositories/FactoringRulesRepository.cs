using FileToSql.Infrastructure.Entities;

namespace FileToSql.Infrastructure.Repositories;

public class FactoringRulesRepository
{
    public async Task<FactoringRules?> GetRuleByFileIdsAsync(List<string> fileIds, CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => new FactoringRules()
        {
            ManufacturerId = Guid.Parse("30e01be6-eabf-4ded-74b6-08da3caf4ddf"),
            FileIds = new()
            {
                "166f86ab-5095-4db4-4e54-08db29eb21d7",
                "61d0de57-6d0e-4e19-4e55-08db29eb21d7"
            },
            CurrencyRate = new()
            {
                CurrencyFrom = Enums.CurrencyType.EUR,
                CurrencyTo = Enums.CurrencyType.GBP,
                Rate = 1.5
            },
            Rules = new()
            {
                new FactoringRules.FactoringRule()
                {
                    Factoring = 2,
                    Operator = Enums.FactoringOperator.GreaterOrEqual,
                    Value = 70
                }
            }
        });
    }
}
