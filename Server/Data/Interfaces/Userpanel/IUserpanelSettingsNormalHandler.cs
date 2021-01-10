using System;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.Userpanel
{
#nullable enable

    public interface IUserpanelSettingsNormalHandler
    {
        Task<string> ConfirmDiscordUserId(ulong discordUserId);
    }
}