using Krecha.Lib.Data;
using Krecha.Lib.Data.Models;
using Krecha.Lib.Services.Requests;
using Krecha.Lib.Services.Responses;

namespace Krecha.Lib.Services;
public class SettlementsService
{
    private readonly Repository<Currency> _currencyRepository;
    private readonly Repository<Settlement> _settlementRepository;
    private readonly Repository<SettlementEntry> _settlementEntryRepository;

    public SettlementsService(Repository<Currency> currencyRepository,
                              Repository<Settlement> settlementRepository,
                              Repository<SettlementEntry> settlementEntryRepository)
    {
        _currencyRepository = currencyRepository;
        _settlementRepository = settlementRepository;
        _settlementEntryRepository = settlementEntryRepository;
    }

    public async Task<CreateSettlementResponse> CreateSettlementAsync(CreateSettlementRequest request)
    {
        Currency? currency = await _currencyRepository.GetById(request.CurrencyId);

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

        await _settlementRepository.Create(toCreate);

        return CreateSettlementResponse.Successful(toCreate.Id);
    }

    public async Task<CreateSettlementEntryResponse> CreateSettlementEntryAsync(CreateSettlementEntryRequest request)
    {
        CreateSettlementEntryResponse response = new();

        Settlement? settlement = await _settlementRepository.GetById(request.SettlementId);

        if (settlement is null)
        {
            return CreateSettlementEntryResponse.Failed();
        }

        SettlementEntry toCreate = new()
        {
            Description = request.Description,
            Amount = request.Amount,
        };

        await _settlementEntryRepository.Create(toCreate);

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

        await _currencyRepository.Create(toCreate);

        return CreateCurrencyResponse.Successful(toCreate.Id);
    }
}
