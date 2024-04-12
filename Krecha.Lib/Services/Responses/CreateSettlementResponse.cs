using Krecha.Lib.Data.Models;

namespace Krecha.Lib.Services.Responses;
public class CreateSettlementResponse
{
    public bool Success { get; set; }
    public int? CreatedSettlementId { get; set; } = null;
}
