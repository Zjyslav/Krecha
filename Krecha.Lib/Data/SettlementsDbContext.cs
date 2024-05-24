using Krecha.Lib.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Krecha.Lib.Data;
public class SettlementsDbContext : DbContext
{
    public DbSet<Settlement> Settlements { get; set; }
    public DbSet<SettlementEntry> Entries { get; set; }
    public DbSet<Currency> Currencies { get; set; }

    public SettlementsDbContext(DbContextOptions<SettlementsDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Primary Keys:
        modelBuilder.Entity<Currency>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Settlement>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<SettlementEntry>()
            .HasKey(e => e.Id);

        // Relationships
        modelBuilder.Entity<Settlement>()
            .HasMany(s => s.Entries)
            .WithOne(e => e.Settlement)
            .HasForeignKey("SettlementId")
            .IsRequired();

        modelBuilder.Entity<Currency>()
            .HasMany(c => c.Settlements)
            .WithOne(s => s.Currency)
            .HasForeignKey("CurrencyId")
            .IsRequired();

        // Seed data:
        modelBuilder.Entity<Currency>()
            .HasData(SeedGenerator.GetCurrencySeed());

        base.OnModelCreating(modelBuilder);
    }
}
