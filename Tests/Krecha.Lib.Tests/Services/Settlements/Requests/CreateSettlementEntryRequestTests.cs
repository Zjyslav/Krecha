using Krecha.Lib.Services.Requests;

namespace Krecha.Lib.Tests.Services.Settlements.Requests;
public class CreateSettlementEntryRequestTests
{
    [Fact]
    public void CreateSettlementEntryRequestConstructor_WhenDescriptionIsNull_ShouldThrowCorrectException()
    {
        // Arrange
        int settlementId = 19;
        string description = null!;
        decimal amount = 11.38m;

        // Act
        CreateSettlementEntryRequest act() => new CreateSettlementEntryRequest(settlementId, description, amount);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal(nameof(description), exception.ParamName);
    }
}
