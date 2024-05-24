using AutoFixture;
using Krecha.Lib.Data;
using Krecha.Lib.Data.Models;
using Krecha.Lib.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Krecha.Lib.Tests.Data;
public class SettlementEntryRepositoryTests
{
    private readonly Repository<SettlementEntry, SettlementsDbContext> _settlementEntryRepository;
    private readonly SettlementsDbContext _dbContext;
    private readonly Fixture _fixture = new();

    public SettlementEntryRepositoryTests()
    {
        _dbContext = EFHelpers.SetupInMemoryDbContext();
        _settlementEntryRepository = new(_dbContext);

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
    public async Task GetById_WhenSettlementEntryDoesntExist_ShouldReturnNull()
    {
        // Arrange
        int settlementEntryId = 3;

        // Act
        var actualSettlementEntry = await _settlementEntryRepository.GetById(settlementEntryId);

        // Assert
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

    [Fact]
    public async Task Create_ForSettlementEntry_ShouldReturnTheSameObject()
    {
        // Arrange
        SettlementEntry expected = CreateTestSettlementEntries(1).First();

        // Act
        var actual = await _settlementEntryRepository.Create(expected);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Create_ForSettlementEntry_ShouldAddItToDb()
    {
        // Arrange
        SettlementEntry expected = CreateTestSettlementEntries(1).First();
        var expectedEntityState = EntityState.Unchanged;

        // Act
        await _settlementEntryRepository.Create(expected);
        var actual = await _dbContext.Entries.FindAsync(expected.Id);
        var actuaEntityState = _dbContext.Entry(expected).State;

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
        Assert.Equal(expectedEntityState, actuaEntityState);
    }

    [Fact]
    public async Task Create_ForSettlementEntryThatExists_ShouldThrowArgumentException()
    {
        // Arrange
        SettlementEntry entryInDb = CreateAndAddToDbTestSettlementEntries(1).First();

        // Act
        var act = () => _settlementEntryRepository.Create(entryInDb);

        // Assert
        await Assert.ThrowsAnyAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task Update_WhenSettlementEntryDoesntExist_ShouldReturnNull()
    {
        // Arrange
        int entryId = 11;

        // Act
        var actual = await _settlementEntryRepository.Update(entryId, entry => { });

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public async Task Update_WhenSettlementEntryExists_ShouldApplyAllChanges()
    {
        // Arrange
        SettlementEntry entry = CreateAndAddToDbTestSettlementEntries(1).First();
        DateTime expectedCreatedDate = DateTime.Now.AddMinutes(-19);
        string expectedDescription = "Test Description";
        decimal expectedAmount = 1138;
        bool expectedArchived = true;

        // Act
        var actual = await _settlementEntryRepository.Update(entry.Id, entry =>
        {
            entry.CreatedDate = expectedCreatedDate;
            entry.Description = expectedDescription;
            entry.Amount = expectedAmount;
            entry.Archived = expectedArchived;
        });

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(expectedCreatedDate, actual.CreatedDate);
        Assert.Equal(expectedDescription, actual.Description);
        Assert.Equal(expectedAmount, actual.Amount);
        Assert.Equal(expectedArchived, actual.Archived);
    }

    [Fact]
    public async Task Update_WhenSettlementEntryExists_ShouldReturnTheSameObject()
    {
        // Arrange
        SettlementEntry expected = CreateAndAddToDbTestSettlementEntries(1).First();

        // Act
        var actual = await _settlementEntryRepository.Update(expected.Id, currency => { });

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Update_WhenSettlementEntryExists_ShouldResultInCorrectEntityState()
    {
        // Arrange
        SettlementEntry entry = CreateAndAddToDbTestSettlementEntries(1).First();
        var expected = EntityState.Unchanged;

        // Act
        var result = await _settlementEntryRepository.Update(entry.Id, currency => { });
        var actual = _dbContext.Entry(result!).State;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Delete_WhenSettlementEntryDoesntExist_ShouldReturnNull()
    {
        // Arrange
        int settlementEntryId = 19;

        // Act
        var actual = await _settlementEntryRepository.Delete(settlementEntryId);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public async Task Delete_WhenSettlementEntryExists_ShouldReturnTheSameObject()
    {
        // Arrange
        SettlementEntry expected = CreateAndAddToDbTestSettlementEntries(1).First();

        // Act
        var actual = await _settlementEntryRepository.Delete(expected.Id);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Delete_WhenSettlementEntryExists_ShouldRemoveItFromDb()
    {
        // Arrange
        SettlementEntry entry = CreateAndAddToDbTestSettlementEntries(1).First();
        var expectedEntityState = EntityState.Detached;

        // Act
        await _settlementEntryRepository.Delete(entry.Id);
        var actual = await _dbContext.Entries.FindAsync(entry.Id);
        var actualEntityState = _dbContext.Entry(entry).State;

        // Assert
        Assert.Null(actual);
        Assert.Equal(expectedEntityState, actualEntityState);
    }

    private List<SettlementEntry> CreateAndAddToDbTestSettlementEntries(int count)
    {
        var entries = CreateTestSettlementEntries(count);
        AddEntitiesToInMemoryDb(entries);
        return entries;
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
