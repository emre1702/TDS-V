using GTANetworkAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Models.GTA;

namespace TDS_Server.RAGE.Player
{
    class Player : IPlayer
    {
        internal readonly GTANetworkAPI.Player _instance;

        internal Player(GTANetworkAPI.Player player)
        {
            _instance = player;
        }

        public string Name => _instance.Name;
        public ulong SocialClubId => _instance.SocialClubId;
        public string SocialClubName => _instance.SocialClubName;

        public Position3D Position 
        { 
            get => new Position3D(_instance.Position.X, _instance.Position.Y, _instance.Position.Z);
            set => _instance.Position = new Vector3(value.X, value.Y, value.Z);
        }

        public ushort RemoteId => _instance.Handle.Value;
        public int Transparency
        {
            get => _instance.Transparency;
            set => _instance.Transparency = value;
        }

        public void SetHealth(int health)
        {
            NAPI.Player.SetPlayerHealth(_instance, health);
        }

        public void SendEvent(string eventName, params object[] args)
        {
            NAPI.ClientEvent.TriggerClientEvent(_instance, eventName, args);
        }
    }
}
