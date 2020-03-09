using GTANetworkAPI;
using TDS_Server.Data.Interfaces.ModAPI;

namespace TDS_Server.RAGE
{
    class Player : IPlayer
    {
        internal GTANetworkAPI.Player Instance;

        internal Player(GTANetworkAPI.Player player)
        {
            Instance = player;
        }

        public void SetHealth(int health)
        {
            NAPI.Player.SetPlayerHealth(Instance, health);
        }
    }
}
