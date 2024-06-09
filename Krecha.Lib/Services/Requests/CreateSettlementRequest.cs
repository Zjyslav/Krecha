namespace Krecha.Lib.Services.Requests;
public class CreateSettlementRequest
{
    public CreateSettlementRequest(string name, string description, int currencyId)
    {
        ValidateConstructorArguments(name, description);

        Name = name;
        Description = description;
        CurrencyId = currencyId;
    }

    public string Name { get; }
    public string Description { get; }
    public int CurrencyId { get; }

    private static void ValidateConstructorArguments(string name, string description)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (description is null)
        {
            throw new ArgumentNullException(nameof(description));
        }
    }
}
