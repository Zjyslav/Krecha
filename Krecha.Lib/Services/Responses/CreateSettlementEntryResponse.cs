namespace Krecha.Lib.Services.Responses;
public class CreateSettlementEntryResponse
{
    public bool Success { get; set; }
    public int? CreatedEntryId { get; set; } = null;

    public static CreateSettlementEntryResponse Failed()
    {
        return new()
        {
            Success = false,
        };
    }

    public static CreateSettlementEntryResponse Successful(int createdEntryId)
    {
        return new()
        {
            Success = true,
            CreatedEntryId = createdEntryId
        };
    }
}
