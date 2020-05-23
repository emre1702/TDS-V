using System;
using System.Threading.Tasks;

namespace TDS_Server.Data.Interfaces.Userpanel
{
    public interface IUserpanelSettingsNormalHandler
    {
        Task<string> ConfirmDiscordUserId(ulong userId);
        Task<object> SaveSettings(ITDSPlayer player, ArraySegment<object> args);
    }
}
