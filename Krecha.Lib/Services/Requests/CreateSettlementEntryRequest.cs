namespace Krecha.Lib.Services.Requests;
public class CreateSettlementEntryRequest
{
    public CreateSettlementEntryRequest(int settlementId, string description, decimal amount)
    {
        SettlementId = settlementId;
        Description = description;
        Amount = amount;
    }

    public int SettlementId { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
}
