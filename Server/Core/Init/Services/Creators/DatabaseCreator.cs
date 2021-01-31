using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;
using TDS.Server.Database;
using TDS.Server.Database.Entity;
using TDS.Server.Handler.Server;

namespace TDS.Server.Core.Init.Services.Creators
{
    internal static class DatabaseCreator
    {
        internal static void CreateDbContextOptionsBuilder(DbContextOptionsBuilder options, AppConfigHandler appConfigHandler, ILoggerFactory? loggerFactory)
        {
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);

            if (loggerFactory is { })
                options.UseLoggerFactory(loggerFactory);

            options.UseNpgsql(appConfigHandler.ConnectionString/*, options =>
                    options
                        // .EnableRetryOnFailure()  DOES NOT WORK WITH TRANSACTIONS => EXCEPTION*/
                )
                .EnableSensitiveDataLogging()
                ;
        }

        internal static IServiceCollection WithDatabase(this IServiceCollection serviceCollection)
        {
            var appConfigHandler = new AppConfigHandler();
#pragma warning disable CA2000 // Dispose objects before losing scope
#pragma warning disable IDE0067 // Dispose objects before losing scope
            var loggerFactory = LoggerFactory.Create(builder =>
                   builder.AddProvider(new CustomDBLogger(appConfigHandler.Logging.Select(s => (s.Level, s.Path))))
               );
#pragma warning restore IDE0067 // Dispose objects before losing scope
#pragma warning restore CA2000 // Dispose objects before losing scope

            return serviceCollection
                .AddSingleton(appConfigHandler)
                .AddDbContext<TDSDbContext>(options => CreateDbContextOptionsBuilder(options, appConfigHandler, loggerFactory), ServiceLifetime.Transient, ServiceLifetime.Singleton);
        }
    }
}