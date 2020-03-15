using GTANetworkAPI;
using System.Diagnostics.CodeAnalysis;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Models.GTA;
using TDS_Server.RAGE.Extensions;

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
        string IPlayer.Name 
        {
            get => _instance.Name;
            set => _instance.Name = value;
        }

        public string IPAddress => _instance.Address;

        public string Serial => _instance.Serial;

        public bool Dead => _instance.Dead;

        public void SetHealth(int health)
        {
            NAPI.Player.SetPlayerHealth(_instance, health);
        }

        public void SendEvent(string eventName, params object[] args)
        {
            NAPI.ClientEvent.TriggerClientEvent(_instance, eventName, args);
        }

        public void Spawn(Position3D position, float heading = 0)
        {
            NAPI.Player.SpawnPlayer(_instance, position.ToVector3(), heading);
        }

        public void SetSkin(Data.Enums.PedHash pedHash)
        {
            NAPI.Player.SetPlayerSkin(_instance, (PedHash)pedHash);
        }

        public void SetVoiceTo(ITDSPlayer target, bool on)
        {
            if (!(target.ModPlayer is Player targetModPlayer))
                return;

            if (on)
                NAPI.Player.EnablePlayerVoiceTo(_instance, targetModPlayer._instance);
            else
                NAPI.Player.DisablePlayerVoiceTo(_instance, targetModPlayer._instance);
        }

        public void Kick(string reason)
        {
            NAPI.Player.KickPlayer(_instance, reason);
        }

        public bool Equals(IPlayer? other)
        {
            return SocialClubId == other?.SocialClubId;
        }
    }
}
