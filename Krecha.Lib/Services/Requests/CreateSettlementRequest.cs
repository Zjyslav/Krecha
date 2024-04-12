using Krecha.Lib.Data.Models;

namespace Krecha.Lib.Services.Requests;
public class CreateSettlementRequest
{
    public CreateSettlementRequest(string name, string description, int currencyId)
    {
        Name = name;
        Description = description;
        CurrencyId = currencyId;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public int CurrencyId { get; set; }
}
