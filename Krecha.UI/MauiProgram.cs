using Krecha.Lib.Data;
using Krecha.Lib.Data.Models;
using Krecha.Lib.Interfaces.Data;
using Microsoft.Extensions.Logging;

namespace Krecha.UI;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

        builder.Services.AddDbContext<SettlementsDbContext>();
        builder.Services.AddScoped<IRepository<Currency>, Repository<Currency, SettlementsDbContext>>();
        builder.Services.AddScoped<IRepository<Settlement>, Repository<Settlement, SettlementsDbContext>>();
        builder.Services.AddScoped<IRepository<SettlementEntry>, Repository<SettlementEntry, SettlementsDbContext>>();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<SettlementsDbContext>();

            // uncomment to start with empty db
            //db.Database.EnsureDeleted();

            db.Database.EnsureCreated();
        }

        return app;
    }
}
