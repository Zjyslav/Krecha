namespace Krecha.Lib.Services.Responses;
public class CreateSettlementResponse
{
    public bool Success { get; set; }
    public int? CreatedSettlementId { get; set; } = null;

    public static CreateSettlementResponse Failed()
    {
        return new()
        {
            Success = false
        };
    }

    public static CreateSettlementResponse Successful(int createdSettlementId)
    {
        return new()
        {
            Success = true,
            CreatedSettlementId = createdSettlementId
        };
    }
}
