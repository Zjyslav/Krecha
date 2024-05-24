using AutoFixture;
using Krecha.Lib.Data;
using Krecha.Lib.Data.Models;
using Krecha.Lib.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Krecha.Lib.Tests.Data;
public class SettlementRepositoryTests
{
    private readonly Repository<Settlement, SettlementsDbContext> _settlementRepository;
    private readonly SettlementsDbContext _dbContext;
    private readonly Fixture _fixture = new();

    public SettlementRepositoryTests()
    {
        _dbContext = EFHelpers.SetupInMemoryDbContext();
        _settlementRepository = new(_dbContext);
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
    public async Task Create_ForSettlement_ShouldReturnTheSameObject()
    {
        // Arrange
        Settlement expected = CreateTestSettlements(1).First();

        // Act
        var actual = await _settlementRepository.Create(expected);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Create_ForSettlement_ShouldAddItToDb()
    {
        // Arrange
        Settlement expected = CreateTestSettlements(1).First();
        var expectedEntityState = EntityState.Unchanged;

        // Act
        await _settlementRepository.Create(expected);
        var actual = await _dbContext.Settlements.FindAsync(expected.Id);
        var actuaEntityState = _dbContext.Entry(expected).State;

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
        Assert.Equal(expectedEntityState, actuaEntityState);
    }
    
    [Fact]
    public async Task Create_ForSettlementThatExists_ShouldThrowArgumentException()
    {
        // Arrange
        Settlement settlementInDb = CreateAndAddToDbTestSettlements(1).First();

        // Act
        var act = () => _settlementRepository.Create(settlementInDb);

        // Assert
        await Assert.ThrowsAnyAsync<ArgumentException>(act);
    }
    
    [Fact]
    public async Task Update_WhenSettlementDoesntExist_ShouldReturnNull()
    {
        // Arrange
        int settlementId = 11;

        // Act
        var actual = await _settlementRepository.Update(settlementId, settlement => { });

        // Assert
        Assert.Null(actual);
    }
    
    [Fact]
    public async Task Update_WhenSettlementExists_ShouldApplyAllChanges()
    {
        // Arrange
        Settlement settlement = CreateAndAddToDbTestSettlements(1).First();
        string expectedName = "Test Name";
        string expectedDescription = "Test Description";
        bool expectedArchived = true;

        // Act
        var actual = await _settlementRepository.Update(settlement.Id, settlement =>
        {
            settlement.Name = expectedName;
            settlement.Description = expectedDescription;
            settlement.Archived = expectedArchived;
        });

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(expectedName, actual.Name);
        Assert.Equal(expectedDescription, actual.Description);
        Assert.Equal(expectedArchived, actual.Archived);
    }
    
    [Fact]
    public async Task Update_WhenSettlementExists_ShouldReturnTheSameObject()
    {
        // Arrange
        Settlement expected = CreateAndAddToDbTestSettlements(1).First();

        // Act
        var actual = await _settlementRepository.Update(expected.Id, currency => { });

        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async Task Update_WhenSettlementExists_ShouldResultInCorrectEntityState()
    {
        // Arrange
        Settlement settlement = CreateAndAddToDbTestSettlements(1).First();
        var expected = EntityState.Unchanged;

        // Act
        var result = await _settlementRepository.Update(settlement.Id, currency => { });
        var actual = _dbContext.Entry(result!).State;

        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async Task Delete_WhenSettlementDoesntExist_ShouldReturnNull()
    {
        // Arrange
        int settlementId = 19;

        // Act
        var actual = await _settlementRepository.Delete(settlementId);

        // Assert
        Assert.Null(actual);
    }
    
    [Fact]
    public async Task Delete_WhenSettlementExists_ShouldReturnTheSameObject()
    {
        // Arrange
        Settlement expected = CreateAndAddToDbTestSettlements(1).First();

        // Act
        var actual = await _settlementRepository.Delete(expected.Id);

        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async Task Delete_WhenSettlementExists_ShouldRemoveItFromDb()
    {
        // Arrange
        Settlement settlement = CreateAndAddToDbTestSettlements(1).First();
        var expectedEntityState = EntityState.Detached;

        // Act
        await _settlementRepository.Delete(settlement.Id);
        var actual = await _dbContext.Settlements.FindAsync(settlement.Id);
        var actualEntityState = _dbContext.Entry(settlement).State;

        // Assert
        Assert.Null(actual);
        Assert.Equal(expectedEntityState, actualEntityState);
    }

    private List<Settlement> CreateAndAddToDbTestSettlements(int count)
    {
        var settlements = CreateTestSettlements(count);
        AddEntitiesToInMemoryDb(settlements);
        return settlements;
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
    
    private void AddEntitiesToInMemoryDb<TEntity>(List<TEntity> entities)
        where TEntity : class
    {
        _dbContext
            .Set<TEntity>()
            .AddRange(entities);

        _dbContext.SaveChanges();
    }
}
