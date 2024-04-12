namespace Krecha.Lib.Services.Responses;
public class CreateCurrencyResponse
{
    public bool Success { get; set; }
    public int? CreatedCurrencyId { get; set; } = null;

    public static CreateCurrencyResponse Failed()
    {
        return new()
        {
            Success = false
        };
    }

    public static CreateCurrencyResponse Successful(int createdCurrencyId)
    {
        return new()
        {
            Success = true,
            CreatedCurrencyId = createdCurrencyId
        };
    }
}
