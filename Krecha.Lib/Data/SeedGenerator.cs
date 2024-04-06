using Krecha.Lib.Data.Models;

namespace Krecha.Lib.Data;
internal static class SeedGenerator
{
    public static Currency[] GetCurrencySeed() => [
                new Currency{ Id = 1, Name = "PLN", Symbol = "zł" , SymbolPosition = CurrencySymbolPosition.After},
                new Currency{ Id = 2, Name="EUR", Symbol = "€" },
                new Currency{ Id = 3, Name="USD", Symbol = "$" }
                ];
}
