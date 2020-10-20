using Microsoft.Extensions.DependencyInjection;
using TDS_Server.Data.Interfaces.MailSystem;
using TDS_Server.MailSystem;

namespace TDS_Server.Core.Init.Services.Creators
{
    internal static class MailCreator
    {
        public static IServiceCollection WithMailsystem(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<IMailSender, MailSender>();
    }
}
