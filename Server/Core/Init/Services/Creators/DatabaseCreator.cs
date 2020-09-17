using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TDS_Server.Database;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Server;

namespace TDS_Server.Core.Init.Services.Creators
{
    internal static class DatabaseCreator
    {
        internal static void CreateDbContextOptionsBuilder(DbContextOptionsBuilder options, AppConfigHandler appConfigHandler, ILoggerFactory? loggerFactory)
        {
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);

            if (loggerFactory is { })
                options.UseLoggerFactory(loggerFactory);

            options.UseNpgsql(appConfigHandler.ConnectionString /*, options =>
                    options.EnableRetryOnFailure()*/)
                .EnableSensitiveDataLogging();
        }

        internal static IServiceCollection WithDatabase(this IServiceCollection serviceCollection)
        {
            var appConfigHandler = new AppConfigHandler();
            var loggerFactory = LoggerFactory.Create(builder =>
                   builder.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Debug)
                       .AddProvider(new CustomDBLogger(@"D:\DBLogs\FromCsharp\log.txt"))
               );

            return serviceCollection
                .AddDbContext<TDSDbContext>(options => CreateDbContextOptionsBuilder(options, appConfigHandler, loggerFactory), ServiceLifetime.Transient, ServiceLifetime.Singleton);
        }
    }
}
