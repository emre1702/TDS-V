using Microsoft.Extensions.DependencyInjection;
using TDS.Server.Data.Interfaces.MailSystem;
using TDS.Server.MailSystem;

namespace TDS.Server.Core.Init.Services.Creators
{
    internal static class MailCreator
    {
        public static IServiceCollection WithMailsystem(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<IMailSender, MailSender>();
    }
}
