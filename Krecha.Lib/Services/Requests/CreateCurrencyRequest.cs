using Krecha.Lib.Data.Models;

namespace Krecha.Lib.Services.Requests;
public class CreateCurrencyRequest
{
    public CreateCurrencyRequest(string name,
                                 string symbol,
                                 CurrencySymbolPosition symbolPosition = CurrencySymbolPosition.Before)
    {
        Name = name;
        Symbol = symbol;
        SymbolPosition = symbolPosition;
    }

    public string Name { get; set; }
    public string Symbol { get; set; }
    public CurrencySymbolPosition SymbolPosition { get; set; }
}
