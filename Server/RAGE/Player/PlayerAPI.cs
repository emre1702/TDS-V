using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces.ModAPI.Player;

namespace TDS_Server.RAGE.Player
{
    class PlayerAPI : IPlayerAPI
    {
        internal Dictionary<GTANetworkAPI.Player, IPlayer> PlayerCache { get; } = new Dictionary<GTANetworkAPI.Player, IPlayer>();

        internal void PlayerConnected(GTANetworkAPI.Player modPlayer)
        {
            var player = new Player(modPlayer);
            PlayerCache[modPlayer] = player;
        }

        internal void PlayerDisconnected(GTANetworkAPI.Player modPlayer)
        {
            PlayerCache.Remove(modPlayer);
        }

        internal IPlayer? GetIPlayer(GTANetworkAPI.Player player)
        {
            PlayerCache.TryGetValue(player, out IPlayer? value);
            return value;
        }


        public IPlayer? GetPlayerByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            foreach (var entry in PlayerCache.Keys)
            {
                if (entry.Name.Equals(name, System.StringComparison.CurrentCultureIgnoreCase))
                    return PlayerCache[entry];
            }

            return null;
        }

        public void SetHealth(IPlayer player, int health)
        {
            if (!((player as Player)?._instance is { } instance))
                return;
            NAPI.Player.SetPlayerHealth(instance, health);
        }


    }
}
