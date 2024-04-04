﻿using Krecha.Lib.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Krecha.Lib.Data;
public class SettlementsDbContext : DbContext
{
    public DbSet<Settlement> Settlements { get; set; }
    public DbSet<SettlementEntry> Entries { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public string DbPath { get; }
    const string _dbName = "krecha.db";

    public SettlementsDbContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, _dbName);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}
