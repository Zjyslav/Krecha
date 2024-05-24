using Krecha.Lib.Data;
using Microsoft.EntityFrameworkCore;
namespace Krecha.Lib.Tests.Helpers;
internal static class EFHelpers
{
    internal static SettlementsDbContext SetupInMemoryDbContext()
    {
        DbContextOptionsBuilder<SettlementsDbContext> dbBuilder = new();
        dbBuilder.UseInMemoryDatabase(databaseName: $"InMemoryDb-{Guid.NewGuid()}");
        SettlementsDbContext context = new(dbBuilder.Options);
        return context;
    }
}
