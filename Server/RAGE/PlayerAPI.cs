using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.RAGE.Events.Custom;

namespace TDS_Server.RAGE
{
    class PlayerAPI : IPlayerAPI
    {
        internal Dictionary<GTANetworkAPI.Player, IPlayer> PlayerCache { get; } = new Dictionary<GTANetworkAPI.Player, IPlayer>();

        public PlayerAPI()
        {
            BaseCustomEvents.PlayerConnectedInternal += (modPlayer) =>
            {
                var player = new Player(modPlayer);
                PlayerCache[modPlayer] = player;
                BaseCustomEvents.PlayerConnected?.Invoke(player);
            };

            BaseCustomEvents.PlayerConnectedInternal += (modPlayer) =>
            {
                var player = new Player(modPlayer);
                PlayerCache[modPlayer] = player;
                BaseCustomEvents.PlayerDisconnected?.Invoke(player);
            };
        }


        public IPlayer GetPlayerByName(string name)
        {

        }

        public void SetHealth(IPlayer player, int health)
        {
            NAPI.Player.SetPlayerHealth((player as Player).Instance, health);
        }
    }
}
