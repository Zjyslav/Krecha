using Krecha.Lib.Data.Models;
using Krecha.Lib.Services.Requests;
using Moq;

namespace Krecha.Lib.Tests.Services.Settlements;
public class CreateSettlementAsyncTests : SettlementsServiceTetstBase
{
    [Fact]
    public async Task CreateSettlementAsync_WhenRequestIsValid_ShouldCreateCorrectSettlement()
    {
        // Arrange
        CreateSettlementRequest request = CreateTestRequest();

        Currency currency = CreateTestCurrency();
        currency.Id = request.CurrencyId;
        SetupMockGetCurrencyById(request.CurrencyId, currency);

        List<Settlement> createdSettlements = SetupCaptureOfCreateSettlement();

        // Act
        await SettlementsService.CreateSettlementAsync(request);
        var actualCreatedSettlement = createdSettlements.Single();

        // Assert
        Assert.NotNull(actualCreatedSettlement);
        Assert.Equal(request.Name, actualCreatedSettlement.Name);
        Assert.Equal(request.Description, actualCreatedSettlement.Description);
        Assert.Equal(request.CurrencyId, actualCreatedSettlement.Currency.Id);
    }

    [Fact]
    public async Task CreateSettlementAsync_WhenRequestIsValid_ShouldReturnCorrectSuccessResponse()
    {
        // Arrange
        CreateSettlementRequest request = CreateTestRequest();

        SetupMockGetCurrencyById(request.CurrencyId, CreateTestCurrency());

        List<Settlement> createdSettlements = SetupCaptureOfCreateSettlement();

        // Act
        var response = await SettlementsService.CreateSettlementAsync(request);
        var actualCreatedSettlement = createdSettlements.Single();

        // Assert
        Assert.True(response.Success);
        Assert.Equal(actualCreatedSettlement.Id, response.CreatedSettlementId);
    }

    [Fact]
    public async Task CreateSettlementAsync_WhenCurrencyDoesntExist_ShoulReturnFailedResponse()
    {
        // Arrange
        CreateSettlementRequest request = CreateTestRequest();

        SetupMockGetCurrencyById(request.CurrencyId, null);

        // Act
        var response = await SettlementsService.CreateSettlementAsync(request);

        // Assert
        Assert.False(response.Success);
        Assert.Null(response.CreatedSettlementId);
    }

    private CreateSettlementRequest CreateTestRequest()
    {
        string name = "Test settlement";
        string description = "Test description";
        int currencyId = 19;
        CreateSettlementRequest testRequest = new(name, description, currencyId);

        return testRequest;
    }

    private void SetupMockGetCurrencyById(int currencyId, Currency? currency)
    {
        MockCurrencyRepository
                    .Setup(x => x.GetById(currencyId))
                    .Returns(Task.FromResult(currency));
    }

    private List<Settlement> SetupCaptureOfCreateSettlement()
    {
        List<Settlement> createdSettlements = new(1);
        MockSettlementRepository
            .Setup(x => x.Create(Capture.In(createdSettlements)))
            .Returns((Settlement settlement) => Task.FromResult<Settlement?>(settlement));
        return createdSettlements;
    }
}
