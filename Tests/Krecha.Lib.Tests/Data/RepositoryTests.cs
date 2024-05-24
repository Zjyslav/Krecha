using AutoFixture;
using Krecha.Lib.Data;
using Krecha.Lib.Data.Models;
using Krecha.Lib.Tests.Helpers;

namespace Krecha.Lib.Tests.Data;
public class RepositoryTests
{
    private readonly Repository<Currency, SettlementsDbContext> _currencyRepository;
    private readonly Repository<Settlement, SettlementsDbContext> _settlementRepository;
    private readonly Repository<SettlementEntry, SettlementsDbContext> _settlementEntryRepository;
    private readonly SettlementsDbContext _dbContext;
    private readonly Fixture _fixture = new();

    public RepositoryTests()
    {
        _dbContext = EFHelpers.SetupInMemoryDbContext();
        _currencyRepository = new(_dbContext);
        _settlementRepository = new(_dbContext);
        _settlementEntryRepository = new(_dbContext);
    }
}
