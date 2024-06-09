using Krecha.Lib.Services.Requests;

namespace Krecha.Lib.Tests.Services.Settlements.Requests;
public class CreateSettlementRequestTests
{
    [Fact]
    public void CreateSettlementRequestConstructor_WhenNameIsNull_ShouldThrowCorrectException()
    {
        // Arrange
        string name = null!;
        string description = "Test description";
        int currencyId = 19;

        // Act
        CreateSettlementRequest act() => new CreateSettlementRequest(name, description, currencyId);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal(nameof(name), exception.ParamName);
    }

    [Fact]
    public void CreateSettlementRequestConstructor_WhenDescriptionIsNull_ShouldThrowCorrectException()
    {
        // Arrange
        string name = "Test name";
        string description = null!;
        int currencyId = 19;

        // Act
        CreateSettlementRequest act() => new CreateSettlementRequest(name, description, currencyId);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal(nameof(description), exception.ParamName);
    }
}
