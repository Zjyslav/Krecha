using AutoFixture;
using Krecha.Lib.Data;
using Krecha.Lib.Data.Models;
using Krecha.Lib.Interfaces.Data;
using Krecha.Lib.Services;
using Krecha.Lib.Tests.Helpers;
using Moq;

namespace Krecha.Lib.Tests.Services.Settlements;
public abstract class SettlementsServiceTetstBase
{
    protected SettlementsService SettlementsService { get; }
    protected Mock<IRepository<Currency>> MockCurrencyRepository { get; } = new();
    protected Mock<IRepository<Settlement>> MockSettlementRepository { get; } = new();
    protected Mock<IRepository<SettlementEntry>> MockSettlementEntryRepository { get; } = new();
    protected SettlementsDbContext DbContext { get; }
    protected Fixture Fixture = new();

    protected SettlementsServiceTetstBase()
    {
        SettlementsService = new(MockCurrencyRepository.Object,
                                 MockSettlementRepository.Object,
                                 MockSettlementEntryRepository.Object);

        DbContext = EFHelpers.SetupInMemoryDbContext();
    }

    protected Currency CreateTestCurrency()
    {
        Currency testCurrency = Fixture
            .Build<Currency>()
                .Without(currency => currency.Settlements)
            .Create();

        return testCurrency;
    }
}
