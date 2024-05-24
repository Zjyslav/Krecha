using AutoFixture;
using Krecha.Lib.Data;
using Krecha.Lib.Data.Models;
using Krecha.Lib.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Krecha.Lib.Tests.Data;
public class CurrencyRepositoryTests
{
    private readonly Repository<Currency, SettlementsDbContext> _currencyRepository;
    private readonly SettlementsDbContext _dbContext;
    private readonly Fixture _fixture = new();

    public CurrencyRepositoryTests()
    {
        _dbContext = EFHelpers.SetupInMemoryDbContext();
        _currencyRepository = new(_dbContext);
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
    public async Task Create_ForCurrency_ShouldReturnTheSameObject()
    {
        // Arrange
        Currency expected = CreateTestCurrencies(1).First();

        // Act
        var actual = await _currencyRepository.Create(expected);

        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async Task Create_ForCurrency_ShouldAddItToDb()
    {
        // Arrange
        Currency expected = CreateTestCurrencies(1).First();
        var expectedEntityState = EntityState.Unchanged;

        // Act
        await _currencyRepository.Create(expected);
        var actual = await _dbContext.Currencies.FindAsync(expected.Id);
        var actuaEntityState = _dbContext.Entry(expected).State;

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
        Assert.Equal(expectedEntityState, actuaEntityState);
    }
    
    [Fact]
    public async Task Create_ForCurrencyThatExists_ShouldThrowArgumentException()
    {
        // Arrange
        Currency currencyInDb = CreateAndAddToDbTestCurrencies(1).First();

        // Act
        var act = () => _currencyRepository.Create(currencyInDb);

        // Assert
        await Assert.ThrowsAnyAsync<ArgumentException>(act);
    }
    
    [Fact]
    public async Task Update_WhenCurrencyDoesntExist_ShouldReturnNull()
    {
        // Arrange
        int currencyId = 11;

        // Act
        var actual = await _currencyRepository.Update(currencyId, currency => { });

        // Assert
        Assert.Null(actual);
    }
    
    [Fact]
    public async Task Update_WhenCurrencyExists_ShouldApplyAllChanges()
    {
        // Arrange
        Currency currency = CreateAndAddToDbTestCurrencies(1).First();
        string expectedName = "Test Name";
        string expectedSymbol = "Test Symbol";
        CurrencySymbolPosition expectedSymbolPosition = CurrencySymbolPosition.After;

        // Act
        var actual = await _currencyRepository.Update(currency.Id, currency =>
        {
            currency.Name = expectedName;
            currency.Symbol = expectedSymbol;
            currency.SymbolPosition = expectedSymbolPosition;
        });

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(expectedName, actual.Name);
        Assert.Equal(expectedSymbol, actual.Symbol);
        Assert.Equal(expectedSymbolPosition, actual.SymbolPosition);
    }

    [Fact]
    public async Task Update_WhenCurrencyExists_ShouldReturnTheSameObject()
    {
        // Arrange
        Currency expected = CreateAndAddToDbTestCurrencies(1).First();

        // Act
        var actual = await _currencyRepository.Update(expected.Id, currency => { });

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Update_WhenCurrencyExists_ShouldResultInCorrectEntityState()
    {
        // Arrange
        Currency currency = CreateAndAddToDbTestCurrencies(1).First();
        var expected = EntityState.Unchanged;

        // Act
        var result = await _currencyRepository.Update(currency.Id, currency => { });
        var actual = _dbContext.Entry(result!).State;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Delete_WhenCurrencyDoesntExist_ShouldReturnNull()
    {
        // Arrange
        int currencyId = 19;

        // Act
        var actual = await _currencyRepository.Delete(currencyId);

        // Assert
        Assert.Null(actual);
    }
    
    [Fact]
    public async Task Delete_WhenCurrencyExists_ShouldReturnTheSameObject()
    {
        // Arrange
        Currency expected = CreateAndAddToDbTestCurrencies(1).First();

        // Act
        var actual = await _currencyRepository.Delete(expected.Id);

        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async Task Delete_WhenCurrencyExists_ShouldRemoveItFromDb()
    {
        // Arrange
        Currency currency = CreateAndAddToDbTestCurrencies(1).First();
        var expectedEntityState = EntityState.Detached;

        // Act
        await _currencyRepository.Delete(currency.Id);
        var actual = await _dbContext.Currencies.FindAsync(currency.Id);
        var actualEntityState = _dbContext.Entry(currency).State;

        // Assert
        Assert.Null(actual);
        Assert.Equal(expectedEntityState, actualEntityState);
    }

    private List<Currency> CreateAndAddToDbTestCurrencies(int count)
    {
        var currencies = CreateTestCurrencies(count);
        AddEntitiesToInMemoryDb(currencies);
        return currencies;
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

    private void AddEntitiesToInMemoryDb<TEntity>(List<TEntity> entities)
        where TEntity : class
    {
        _dbContext
            .Set<TEntity>()
            .AddRange(entities);

        _dbContext.SaveChanges();
    }
}
