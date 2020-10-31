using Microsoft.Extensions.DependencyInjection;
using TDS_Server.DamageSystem;
using TDS_Server.Data.Interfaces.DamageSystem;

namespace TDS_Server.Core.Init.Services.Creators
{
    internal static class DamageCreator
    {
        public static IServiceCollection WithDamage(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddTransient<IDamageHandler, DamageHandler>();
    }
}
