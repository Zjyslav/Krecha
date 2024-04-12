using Krecha.Lib.Data;

namespace Krecha.Lib.Services;
public class SettlementsService
{
    private readonly SettlementsDbContext _dbContext;

    public SettlementsService(SettlementsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
