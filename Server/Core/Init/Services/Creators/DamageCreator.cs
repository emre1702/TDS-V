using Microsoft.Extensions.DependencyInjection;
using TDS.Server.DamageSystem;
using TDS.Server.Data.Interfaces.DamageSystem;

namespace TDS.Server.Core.Init.Services.Creators
{
    internal static class DamageCreator
    {
        public static IServiceCollection WithDamage(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddTransient<IDamageHandler, DamageHandler>();
    }
}
