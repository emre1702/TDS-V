using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TDS_Server.Core.Manager.EventManager;
using TDS_Server.Core.Player.Join;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Database;
using TDS_Server.Database.Entity;
using TDS_Server.Handler;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Player;
using TDS_Server.Handler.Sync;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Core.Startup
{
    internal static class Services
    {
        internal static ServiceProvider InitServiceCollection(IModAPI modAPI)
        {
            var appConfigHandler = new AppConfigHandler();

            var loggerFactory = LoggerFactory.Create(builder =>
                    builder.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Debug)
                        .AddProvider(new CustomDBLogger(@"D:\DBLogs\FromCsharp\log.txt"))
                );

            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddSingleton(modAPI)
                .AddSingleton<ConnectedHandler>()
                .AddSingleton<TDSPlayerHandler>()
                .AddSingleton<EventsHandler>()
                .AddSingleton<MappingHandler>()
                .AddSingleton<BansHandler>()
                .AddSingleton<LangHelper>()
                .AddSingleton<LoggingHandler>()
                .AddSingleton<Serializer>()
                .AddSingleton<DataSyncHandler>()

                .AddDbContext<TDSDbContext>(options =>
                    options
                        .UseLoggerFactory(loggerFactory)
                        // .EnableSensitiveDataLogging()
                        .UseNpgsql(appConfigHandler.ConnectionString, options =>
                            options.EnableRetryOnFailure())
                        .UseSnakeCaseNamingConvention()
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
                    );

            return serviceCollection.BuildServiceProvider();
        }


    }
}
