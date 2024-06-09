using Krecha.Lib.Data.Models;
using System.ComponentModel;

namespace Krecha.Lib.Services.Requests;
public class CreateCurrencyRequest
{
    public CreateCurrencyRequest(string name,
                                 string symbol,
                                 CurrencySymbolPosition symbolPosition = CurrencySymbolPosition.Before)
    {
        ValidateConstructorArguments(name, symbol, symbolPosition);

        Name = name;
        Symbol = symbol;
        SymbolPosition = symbolPosition;
    }

    public string Name { get; }
    public string Symbol { get; }
    public CurrencySymbolPosition SymbolPosition { get; }

    private static void ValidateConstructorArguments(string name, string symbol, CurrencySymbolPosition symbolPosition)
    {
        if (Enum.IsDefined(typeof(CurrencySymbolPosition), symbolPosition) == false)
        {
            throw new InvalidEnumArgumentException(nameof(symbolPosition), (int)symbolPosition, typeof(CurrencySymbolPosition));
        }

        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (symbol is null)
        {
            throw new ArgumentNullException(nameof(symbol));
        }
    }
}
