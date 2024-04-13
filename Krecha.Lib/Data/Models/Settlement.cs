namespace Krecha.Lib.Data.Models;
public class Settlement
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = "";
    public required Currency Currency { get; set; }
    public ICollection<SettlementEntry> Entries { get; set; } = new List<SettlementEntry>();
    public bool Archived { get; set; } = false;
}
