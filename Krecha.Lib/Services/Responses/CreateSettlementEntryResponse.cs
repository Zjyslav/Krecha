namespace Krecha.Lib.Services.Responses;
public class CreateSettlementEntryResponse
{
    public bool Success { get; set; }
    public int? CreatedEntryId { get; set; } = null;
}
