using FileToSql.Infrastructure.Entities;

namespace FileToSql;

public static class FusedPriceExtensions
{
    public static FusedPrice ApplyFactoringRules(this FusedPrice price, FactoringRules priceModifiers)
    {
        foreach (var rule in priceModifiers.Rules)
        {
            // do some logic
        }

        price.ListPrice *= priceModifiers.CurrencyRate.Rate;

        return price;
    }
}
