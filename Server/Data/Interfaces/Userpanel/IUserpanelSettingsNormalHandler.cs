using System;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.Userpanel
{
    #nullable enable
    public interface IUserpanelSettingsNormalHandler
    {

        Task<string> ConfirmDiscordUserId(ulong userId);

        Task<object?> SaveSettings(ITDSPlayer player, ArraySegment<object> args);
        object? LoadSettings(ITDSPlayer player, ref ArraySegment<object> args);
    }
}
