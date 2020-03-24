using GTANetworkAPI;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Server.RAGE.Extensions;
using TDS_Shared.Data.Models.GTA;

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
            set => value.ToVector3();
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
        public int VehicleSeat => _instance.VehicleSeat;

        public string IPAddress => _instance.Address;

        public string Serial => _instance.Serial;

        public bool Dead => _instance.Dead;

        public TDS_Shared.Data.Enums.WeaponHash CurrentWeapon
        {
            get => (TDS_Shared.Data.Enums.WeaponHash)_instance.CurrentWeapon;
            set => NAPI.Player.SetPlayerCurrentWeapon(_instance, (GTANetworkAPI.WeaponHash)value);

        }
        public uint Dimension 
        { 
            get => _instance.Dimension; 
            set => _instance.Dimension = value; 
        }
        public int Armor 
        { 
            get => _instance.Armor;
            set => _instance.Armor = value;
        }
        public int Health
        {
            get => _instance.Health;
            set => _instance.Health = value;
        }
        public float Rotation
        {
            get => _instance.Rotation.Z;
            set => _instance.Rotation = new Vector3(0, 0, value);
        }

        public bool IsInVehicle => throw new System.NotImplementedException();

        public IVehicle Vehicle => throw new System.NotImplementedException();

        public bool IsDead => throw new System.NotImplementedException();

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

        public void StopAnimation()
        {
            _instance.StopAnimation();
        }

        public void Kill()
        {
            _instance.Kill();
        }

        public void SetInvincible(bool v)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveAllWeapons()
        {
            throw new System.NotImplementedException();
        }

        public void SetCollisionless(bool v, ILobby lobby)
        {
            throw new System.NotImplementedException();
        }

        public void GiveWeapon(TDS_Shared.Data.Enums.WeaponHash hash, int ammo = 0)
        {
            throw new System.NotImplementedException();
        }

        public void SetWeaponAmmo(TDS_Shared.Data.Enums.WeaponHash hash, int ammo)
        {
            throw new System.NotImplementedException();
        }

        public void PlayAnimation(string v1, string v2, int loop)
        {
            throw new System.NotImplementedException();
        }

        public void Freeze(bool v)
        {
            throw new System.NotImplementedException();
        }

        public void SendMessage(string msg)
        {
            throw new System.NotImplementedException();
        }

        public void SendNotification(string msg, bool flashing)
        {
            throw new System.NotImplementedException();
        }

        public void WarpOutOfVehicle()
        {
            throw new System.NotImplementedException();
        }

        public void SetIntoVehicle(IVehicle vehicle, int v)
        {
            throw new System.NotImplementedException();
        }

        public void SetClothes(int slot, int drawable, int texture)
        {
            throw new System.NotImplementedException();
        }
    }
}
