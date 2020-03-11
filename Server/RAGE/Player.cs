using GTANetworkAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;

namespace TDS_Server.RAGE
{
    class Player : IPlayer
    {
        internal GTANetworkAPI.Player _instance;

        internal Player(GTANetworkAPI.Player player)
        {
            _instance = player;
        }

        public string Name => _instance.Name;
        public ulong SocialClubId => _instance.SocialClubId;
        public string SocialClubName => _instance.SocialClubName;

        public void SetHealth(int health)
        {
            NAPI.Player.SetPlayerHealth(_instance, health);
        }
    }
}
