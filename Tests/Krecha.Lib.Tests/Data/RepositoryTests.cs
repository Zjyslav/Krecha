using AutoFixture;
using Krecha.Lib.Data;
using Krecha.Lib.Data.Models;
using Krecha.Lib.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

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

    [Fact]
    public async Task GetAll_ForCurrencies_ShouldReturnAllItemsInDb()
    {
        // Arrange
        int expectedCurrencyCount = 10;
        var currencies = CreateAndAddToDbTestCurrencies(expectedCurrencyCount);

        // Act
        var actualCurrencies = _currencyRepository.GetAll();
        int actualCurrencyCount = await actualCurrencies.CountAsync();
       
        // Assert
        Assert.Equal(expectedCurrencyCount, actualCurrencyCount);
        
        foreach(var expected in currencies)
        {
            var actual = await actualCurrencies.FirstOrDefaultAsync(currency => currency.Id == expected.Id);

            Assert.NotNull(actual);
            Assert.Equivalent(expected, actual);
        }
    }

    [Fact]
    public async Task GetAll_ForSettlements_ShouldReturnAllItemsInDb()
    {
        // Arrange
        int expectedSettlementCount = 10;
        var settlements = CreateAndAddToDbTestSettlements(expectedSettlementCount);

        // Act
        var actualSettlements = _settlementRepository.GetAll();
        int actualSettlementCount = await actualSettlements.CountAsync();

        // Assert
        Assert.Equal(expectedSettlementCount, actualSettlementCount);

        foreach (var expected in settlements)
        {
            var actual = await actualSettlements.FirstOrDefaultAsync(settlement => settlement.Id == expected.Id);

            Assert.NotNull(actual);
            Assert.Equivalent(expected, actual);
        }
    }
    
    [Fact]
    public async Task GetAll_ForSettlementEntries_ShouldReturnAllItemsInDb()
    {
        // Arrange
        int expectedSettlementEntryCount = 10;
        var settlementEntries = CreateAndAddToDbTestSettlementEntries(expectedSettlementEntryCount);

        // Act
        var actualSettlementEntries = _settlementEntryRepository.GetAll();
        int actualSettlementEntryCount = await actualSettlementEntries.CountAsync();

        // Assert
        Assert.Equal(expectedSettlementEntryCount, actualSettlementEntryCount);

        foreach (var expected in settlementEntries)
        {
            var actual = await actualSettlementEntries.FirstOrDefaultAsync(entry => entry.Id == expected.Id);

            Assert.NotNull(actual);
            Assert.Equivalent(expected, actual);
        }
    }

    [Fact]
    public async Task GetById_WhenCurrencyDoesntExist_ShouldReturnNull()
    {
        // Arrange
        int currencyId = 1;

        // Act
        var actualCurrency = await _currencyRepository.GetById(currencyId);

        // Assert
        Assert.Null(actualCurrency);
    }
    
    [Fact]
    public async Task GetById_WhenSettlementDoesntExist_ShouldReturnNull()
    {
        // Arrange
        int settlementId = 2;

        // Act
        var actualSettlement = await _settlementRepository.GetById(settlementId);

        // Assert
        Assert.Null(actualSettlement);
    }
    
    [Fact]
    public async Task GetById_WhenSettlementEntryDoesntExist_ShouldReturnNull()
    {
        // Arrange
        int settlementEntryId = 3;

        // Act
        var actualSettlementEntry = await _settlementEntryRepository.GetById(settlementEntryId);

        // Assert
    }

    [Fact]
    public async Task GetById_WhenCurrencyExists_ShouldReturnIt()
    {
        // Arrage
        Currency expected = CreateAndAddToDbTestCurrencies(1).First();

        // Act
        var actual = await _currencyRepository.GetById(expected.Id);

        // Assert
        Assert.NotNull(actual);
        Assert.Equivalent(expected, actual);
    }
    
    [Fact]
    public async Task GetById_WhenSettlementExists_ShouldReturnIt()
    {
        // Arrage
        Settlement expected = CreateAndAddToDbTestSettlements(1).First();

        // Act
        var actual = await _settlementRepository.GetById(expected.Id);

        // Assert
        Assert.NotNull(actual);
        Assert.Equivalent(expected, actual);
    }
    
    [Fact]
    public async Task GetById_WhenSettlementEntryExists_ShouldReturnIt()
    {
        // Arrage
        SettlementEntry expected = CreateAndAddToDbTestSettlementEntries(1).First();

        // Act
        var actual = await _settlementEntryRepository.GetById(expected.Id);

        // Assert
        Assert.NotNull(actual);
        Assert.Equivalent(expected, actual);
    }

    private List<Currency> CreateAndAddToDbTestCurrencies(int count)
    {
        var currencies = CreateTestCurrencies(count);
        AddEntitiesToInMemoryDb(currencies);
        return currencies;
    }

    private List<Settlement> CreateAndAddToDbTestSettlements(int count)
    {
        var settlements = CreateTestSettlements(count);
        AddEntitiesToInMemoryDb(settlements);
        return settlements;
    }

    private List<SettlementEntry> CreateAndAddToDbTestSettlementEntries(int count)
    {
        var entries = CreateTestSettlementEntries(count);
        AddEntitiesToInMemoryDb(entries);
        return entries;
    }

    private List<Currency> CreateTestCurrencies(int count)
    {
        List<Currency> output = _fixture
            .Build<Currency>()
                .Without(currency => currency.Id)
                .Without(currency => currency.Settlements)
            .CreateMany(count)
            .ToList();

        return output;
    }
    
    private List<Settlement> CreateTestSettlements(int count)
    {
        List<Settlement> output = _fixture
            .Build<Settlement>()
                .Without(settlement => settlement.Id)
                .Without(settlement => settlement.Entries)
                .Without(settlement => settlement.Currency)
            .CreateMany(count)
            .ToList();

        return output;
    }
    
    private List<SettlementEntry> CreateTestSettlementEntries(int count)
    {
        List<SettlementEntry> output = _fixture
            .Build<SettlementEntry>()
                .Without(entry => entry.Id)
                .Without(entry => entry.Settlement)
            .CreateMany(count)
            .ToList();

        return output;
    }

    private void AddEntitiesToInMemoryDb<TEntity>(List<TEntity> entities)
        where TEntity : class
    {
        _dbContext
            .Set<TEntity>()
            .AddRange(entities);

        _dbContext.SaveChanges();
    }
}
