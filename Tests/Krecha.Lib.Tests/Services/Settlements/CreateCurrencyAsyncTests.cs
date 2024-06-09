using Krecha.Lib.Data.Models;
using Krecha.Lib.Services.Requests;
using Moq;

namespace Krecha.Lib.Tests.Services.Settlements;
public class CreateCurrencyAsyncTests : SettlementsServiceTetstBase
{
    [Fact]
    public async Task CreateCurrencyAsync_WhenRequestIsValid_ShouldCreateCorrectCurrency()
    {
        // Arrange
        CreateCurrencyRequest request = CreateTestRequest();
        List<Currency> createdCurrencies = SetupCaptureOfCurrency();

        // Act
        await SettlementsService.CreateCurrencyAsync(request);
        var actualCreatedCurrency = createdCurrencies.Single();

        // Assert
        Assert.NotNull(actualCreatedCurrency);
        Assert.Equal(request.Name, actualCreatedCurrency.Name);
        Assert.Equal(request.Symbol, actualCreatedCurrency.Symbol);
        Assert.Equal(request.SymbolPosition, actualCreatedCurrency.SymbolPosition);
    }

    [Fact]
    public async Task CreateCurrencyAsync_WhenRequestIsValid_ShouldReturnCorrectSuccessResponse()
    {
        // Arrange
        CreateCurrencyRequest request = CreateTestRequest();
        List<Currency> createdCurrencies = SetupCaptureOfCurrency();

        // Act
        var response = await SettlementsService.CreateCurrencyAsync(request);
        var actualCreatedCurrency = createdCurrencies.Single();

        // Assert
        Assert.True(response.Success);
        Assert.Equal(actualCreatedCurrency.Id, response.CreatedCurrencyId);
    }

    private CreateCurrencyRequest CreateTestRequest()
    {
        string name = "Test currency name";
        string symbol = "Test symbol";
        CurrencySymbolPosition symbolPosition = CurrencySymbolPosition.Before;
        CreateCurrencyRequest testRequest = new(name, symbol, symbolPosition);

        return testRequest;
    }

    private List<Currency> SetupCaptureOfCurrency()
    {
        List<Currency> createdCurrencies = new(1);
        MockCurrencyRepository
            .Setup(x => x.Create(Capture.In(createdCurrencies)))
            .Returns((Currency currency) => Task.FromResult<Currency?>(currency));
        return createdCurrencies;
    }

}
