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
        CreateSettlementResponse response = new();

        Currency? currency = _dbContext.Currencies
            .FirstOrDefault(c => c.Id == request.CurrencyId);

        if (currency is null)
        {
            response.Success = false;
            return response;
        }

        Settlement toCreate = new()
        {
            Name = request.Name,
            Description = request.Description,
            Currency = currency,
        };

        await _dbContext.Settlements.AddAsync(toCreate);
        await _dbContext.SaveChangesAsync();

        response.Success = true;
        response.CreatedSettlementId = toCreate.Id;
        return response;
    }

    public async Task<CreateSettlementEntryResponse> CreateSettlementEntryAsync(CreateSettlementEntryRequest request)
    {
        CreateSettlementEntryResponse response = new();

        Settlement? settlement = _dbContext.Settlements
            .FirstOrDefault(s => s.Id == request.SettlementId);

        if (settlement is null)
        {
            response.Success = false;
            return response;
        }

        SettlementEntry toCreate = new()
        {
            Description = request.Description,
            Amount = request.Amount,
        };

        settlement.Entries.Add(toCreate);
        await _dbContext.SaveChangesAsync();

        response.Success = true;
        response.CreatedEntryId = toCreate.Id;
        return response;
    }

    public async Task<CreateCurrencyResponse> CreateCurrencyAsync(CreateCurrencyRequest request)
    {
        Currency toCreate = new()
        {
            Name = request.Name,
            Symbol = request.Symbol,
            SymbolPosition = request.SymbolPosition,
        };

        await _dbContext.Currencies.AddAsync(toCreate);
        await _dbContext.SaveChangesAsync();

        return CreateCurrencyResponse.Successful(toCreate.Id);
    }
}
