using System;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.Userpanel
{
    #nullable enable
    public interface IUserpanelSettingsNormalHandler
    {

        Task<string> ConfirmDiscordUserId(ulong discordUserId);

        Task<object?> SaveSettings(ITDSPlayer player, ArraySegment<object> args);
        object? LoadSettings(ITDSPlayer player, ref ArraySegment<object> args);
    }
}
