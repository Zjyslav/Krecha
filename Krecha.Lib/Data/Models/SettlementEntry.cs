namespace Krecha.Lib.Data.Models;
public class SettlementEntry
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string Description { get; set; } = "";
    public decimal Amount { get; set; }
    public bool Archived { get; set; } = false;
    public Settlement Settlement { get; set; } = null!;
}
