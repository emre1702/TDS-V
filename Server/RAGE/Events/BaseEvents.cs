using GTANetworkAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.RAGE.Player;
using TDS_Server.RAGE.Startup;

namespace TDS_Server.RAGE.Events
{
    class BaseEvents : Script
    {
        public IPlayer? GetModPlayer(GTANetworkAPI.Player player)
        {
            return (Program.BaseAPI.Player as PlayerAPI)?.GetIPlayer(player);
        }
    }
}
