using DataSyncToDoClientApp.MAUI.Models;
using DataSyncToDoClientApp.MAUI.Services;
using DataSyncToDoClientApp.MAUI.ViewModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataSyncToDoClientApp.MAUI;

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
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        var dbConnection = new SqliteConnection("Data Source=:memory:");
        dbConnection.Open();

        builder.Services.AddTransient<MainPage>()
                        .AddTransient<MainViewModel>()
                        .AddTransient<IAlertService, AlertService>()
                        .AddScoped<IDbInitializer, DbContextInitializer>()
                        .AddDbContext<AppDbContext>(options => options.UseSqlite(dbConnection));

        return builder.Build();
    }
}
