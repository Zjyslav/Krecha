using Krecha.Lib.Data.Models;
using Krecha.Lib.Services.Requests;
using Moq;

namespace Krecha.Lib.Tests.Services.Settlements;
public class CreateSettlementEntryTests : SettlementsServiceTetstBase
{
    [Fact]
    public async Task CreateSettlementEntryAsync_WhenRequestIsValid_ShouldCreateCorrectSettlement()
    {
        // Arrange
        CreateSettlementEntryRequest request = CreateTestRequest();

        Settlement settlement = CreateTestSettlement();
        settlement.Id = request.SettlementId;
        SetupMockGetSettlementById(request.SettlementId, settlement);

        List<SettlementEntry> createdEntries = SetupCaptureOfCreatedEntries();

        // Act
        await SettlementsService.CreateSettlementEntryAsync(request);
        var actualCreatedEntry = createdEntries.Single();

        // Assert
        Assert.NotNull(actualCreatedEntry);
        Assert.Equal(request.SettlementId, actualCreatedEntry.Settlement.Id);
        Assert.Equal(request.Description, actualCreatedEntry.Description);
        Assert.Equal(request.Amount, actualCreatedEntry.Amount);
    }

    [Fact]
    public async Task CreateSettlementEntryAsync_WhenRequestIsValid_ShouldReturnCorrectSuccessResponse()
    {
        // Arrange
        CreateSettlementEntryRequest request = CreateTestRequest();

        SetupMockGetSettlementById(request.SettlementId, CreateTestSettlement());

        List<SettlementEntry> createdEntries = SetupCaptureOfCreatedEntries();

        // Act
        var response = await SettlementsService.CreateSettlementEntryAsync(request);
        var actualCreatedEntry = createdEntries.Single();

        // Assert
        Assert.True(response.Success);
        Assert.Equal(actualCreatedEntry.Id, response.CreatedEntryId);
    }

    [Fact]
    public async Task CreateSettlementEntryAsync_WhenSettlementDoesntExist_ShouldReturnFailedResponse()
    {
        // Arrange
        CreateSettlementEntryRequest request = CreateTestRequest();

        SetupMockGetSettlementById(request.SettlementId, null);

        // Act
        var response = await SettlementsService.CreateSettlementEntryAsync(request);

        // Assert
        Assert.False(response.Success);
        Assert.Null(response.CreatedEntryId);
    }

    private CreateSettlementEntryRequest CreateTestRequest()
    {
        int settlementId = 1;
        string description = "Test description";
        decimal amount = 11.38m;
        CreateSettlementEntryRequest testRequest = new(settlementId, description, amount);

        return testRequest;
    }

    private void SetupMockGetSettlementById(int settlementId, Settlement? settlement)
    {
        MockSettlementRepository
                    .Setup(x => x.GetById(settlementId))
                    .Returns(Task.FromResult(settlement));
    }

    private List<SettlementEntry> SetupCaptureOfCreatedEntries()
    {
        List<SettlementEntry> createdEntries = new(1);
        MockSettlementEntryRepository
            .Setup(x => x.Create(Capture.In(createdEntries)))
            .Returns((SettlementEntry entry) => Task.FromResult<SettlementEntry?>(entry));
        return createdEntries;
    }
}
