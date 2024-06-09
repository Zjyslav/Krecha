using Krecha.Lib.Data.Models;
using Krecha.Lib.Services.Requests;
using System.ComponentModel;

namespace Krecha.Lib.Tests.Services.Settlements.Requests;
public class CreateCurrencyRequestTests
{
    [Fact]
    public void CreateCurrencyRequestConstructor_WhenNameIsNull_ShouldThrowCorrectException()
    {
        // Arrange
        string name = null!;
        string symbol = "Test symbol";
        CurrencySymbolPosition symbolPosition = CurrencySymbolPosition.After;

        // Act
        CreateCurrencyRequest act() => new CreateCurrencyRequest(name, symbol, symbolPosition);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal(nameof(name), exception.ParamName);
    }

    [Fact]
    public void CreateCurrencyRequestConstructor_WhenSymbolIsNull_ShouldThrowCorrectException()
    {
        // Arrange
        string name = "Test name";
        string symbol = null!;
        CurrencySymbolPosition symbolPosition = CurrencySymbolPosition.After;

        // Act
        CreateCurrencyRequest act() => new CreateCurrencyRequest(name, symbol, symbolPosition);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal(nameof(symbol), exception.ParamName);
    }

    [Fact]
    public void CreateCurrencyRequestConstructor_WhenSymbolPositionIsInvalid_ShouldThrowCorrectException()
    {
        // Arrange
        string name = "Test name";
        string symbol = "Test symbol";
        int incorrectEnumValue = -1;
        CurrencySymbolPosition symbolPosition = (CurrencySymbolPosition)incorrectEnumValue;

        // Act
        CreateCurrencyRequest act() => new CreateCurrencyRequest(name, symbol, symbolPosition);

        // Assert
        var exception = Assert.Throws<InvalidEnumArgumentException>(act);
        Assert.Equal(nameof(symbolPosition), exception.ParamName);
    }
}
