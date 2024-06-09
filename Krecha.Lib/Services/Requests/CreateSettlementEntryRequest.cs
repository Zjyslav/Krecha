namespace Krecha.Lib.Services.Requests;
public class CreateSettlementEntryRequest
{
    public CreateSettlementEntryRequest(int settlementId, string description, decimal amount)
    {
        ValidateConstructorArguments(description);

        SettlementId = settlementId;
        Description = description;
        Amount = amount;
    }

    public int SettlementId { get; }
    public string Description { get; }
    public decimal Amount { get; }

    private static void ValidateConstructorArguments(string description)
    {
        if (description is null)
        {
            throw new ArgumentNullException(nameof(description));
        }
    }
}
