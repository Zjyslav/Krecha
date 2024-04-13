using Krecha.Lib.Data;
using Krecha.Lib.Data.Models;
using Krecha.Lib.Services.Requests;
using Krecha.Lib.Services.Responses;

namespace Krecha.Lib.Services;
public class SettlementsService
{
    private readonly SettlementsDbContext _dbContext;

    public SettlementsService(SettlementsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CreateSettlementResponse> CreateSettlementAsync(CreateSettlementRequest request)
    {
        Currency? currency = _dbContext.Currencies
            .FirstOrDefault(c => c.Id == request.CurrencyId);

        if (currency is null)
        {
            return CreateSettlementResponse.Failed();
        }

        Settlement toCreate = new()
        {
            Name = request.Name,
            Description = request.Description,
            Currency = currency,
        };

        _dbContext.Settlements.Add(toCreate);
        await _dbContext.SaveChangesAsync();

        return CreateSettlementResponse.Successful(toCreate.Id);
    }

    public async Task<CreateSettlementEntryResponse> CreateSettlementEntryAsync(CreateSettlementEntryRequest request)
    {
        CreateSettlementEntryResponse response = new();

        Settlement? settlement = _dbContext.Settlements
            .FirstOrDefault(s => s.Id == request.SettlementId);

        if (settlement is null)
        {
            return CreateSettlementEntryResponse.Failed();
        }

        SettlementEntry toCreate = new()
        {
            Description = request.Description,
            Amount = request.Amount,
        };

        settlement.Entries.Add(toCreate);
        await _dbContext.SaveChangesAsync();

        return CreateSettlementEntryResponse.Successful(toCreate.Id);
    }

    public async Task<CreateCurrencyResponse> CreateCurrencyAsync(CreateCurrencyRequest request)
    {
        Currency toCreate = new()
        {
            Name = request.Name,
            Symbol = request.Symbol,
            SymbolPosition = request.SymbolPosition,
        };

        _dbContext.Currencies.Add(toCreate);
        await _dbContext.SaveChangesAsync();

        return CreateCurrencyResponse.Successful(toCreate.Id);
    }
}
