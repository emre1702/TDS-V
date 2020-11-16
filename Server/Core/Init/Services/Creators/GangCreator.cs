using Microsoft.Extensions.DependencyInjection;
using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.GangsSystem;

namespace TDS.Server.Core.Init.Services.Creators
{
    internal static class GangCreator
    {
        public static IServiceCollection WithGang(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<IGangsProvider, GangsProvider>()
                .AddTransient<IGang, Gang>();
    }
}
