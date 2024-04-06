namespace Krecha.Lib.Data.Models;
public class Currency
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Symbol { get; set; }
    public CurrencySymbolPosition SymbolPosition { get; set; } = CurrencySymbolPosition.Before;
}
public enum CurrencySymbolPosition
{
    Before,
    After
}