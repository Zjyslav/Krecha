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

    public async Task<CreateSettlementResponse> CreateSettlement(CreateSettlementRequest request)
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

        response.CreatedSettlementId = toCreate.Id;
        return response;
    }
}
