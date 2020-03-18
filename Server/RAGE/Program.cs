using GTANetworkAPI;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.RAGE.Player;

namespace TDS_Server.RAGE.Startup
{
    class Program : Script
    {
        #nullable disable warnings
        public static BaseAPI BaseAPI;
        public static Core.Startup.Program TDSCore;

        public Program()
        {
            BaseAPI = new BaseAPI();

            TDSCore = new Core.Startup.Program(BaseAPI);
        }

        internal static ITDSPlayer? GetTDSPlayer(GTANetworkAPI.Player player)
        {
            var modPlayer = GetModPlayer(player);
            if (modPlayer is null)
                return null;

            return GetTDSPlayer(modPlayer);
        }

        internal static ITDSPlayer? GetTDSPlayer(IPlayer player)
        {
            return TDSCore.GetTDSPlayer(player);
        }

        internal static IPlayer? GetModPlayer(GTANetworkAPI.Player player)
        {
            return (BaseAPI.Player as PlayerAPI)?.GetIPlayer(player);
        }

    }
}
