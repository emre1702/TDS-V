using Microsoft.Extensions.DependencyInjection;
using TDS_Server.Data.Interfaces.GangsSystem;
using TDS_Server.GangsSystem;

namespace TDS_Server.Core.Init.Services.Creators
{
    internal static class GangCreator
    {
        public static IServiceCollection WithGang(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<IGangsProvider, GangsProvider>()
                .AddTransient<IGang, Gang>();
    }
}
