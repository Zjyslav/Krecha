using Krecha.Lib.Data;
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
