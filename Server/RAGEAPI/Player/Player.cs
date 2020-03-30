using GTANetworkAPI;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Server.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGEAPI.Player
{
    class Player : Entity.Entity, IPlayer
    {
        internal readonly GTANetworkAPI.Player _instance;

        internal Player(GTANetworkAPI.Player instance) : base(instance)
        {
            _instance = instance;
        }

        public string Name => _instance.Name;
        public ulong SocialClubId => _instance.SocialClubId;
        public string SocialClubName => _instance.SocialClubName;

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

        public bool IsInVehicle => _instance.IsInVehicle;

        public IVehicle Vehicle => new Vehicle.Vehicle(_instance.Vehicle);

        public bool IsDead => _instance.Dead;

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

        public void Freeze(bool toggle)
        {
            Init.WorkaroundsHandler.FreezePlayer(_instance, toggle);
        }

        public void SetInvincible(bool toggle)
        {
            Init.WorkaroundsHandler.SetPlayerInvincible(_instance, toggle);
        }

        public void RemoveAllWeapons()
        {
            _instance.RemoveAllWeapons();
        }

        public void GiveWeapon(TDS_Shared.Data.Enums.WeaponHash hash, int ammo = 0)
        {
            _instance.GiveWeapon((WeaponHash)hash, ammo);
        }

        public void SetWeaponAmmo(TDS_Shared.Data.Enums.WeaponHash hash, int ammo)
        {
            _instance.SetWeaponAmmo((WeaponHash)hash, ammo);
        }

        public void PlayAnimation(string animDict, string animName, int flag)
        {
            _instance.PlayAnimation(animDict, animName, flag);
        }

        public void SendMessage(string msg)
        {
            _instance.SendChatMessage(msg);
        }

        public void SendNotification(string msg, bool flashing = false)
        {
            _instance.SendNotification(msg, flashing);
        }

        public void WarpOutOfVehicle()
        {
            _instance.WarpOutOfVehicle();
        }

        public void SetIntoVehicle(IVehicle vehicle, int seat)
        {
            if (!(vehicle is Vehicle.Vehicle veh))
                return;
            _instance.SetIntoVehicle(veh._instance, seat);
        }

        public void SetClothes(int slot, int drawable, int texture)
        {
            _instance.SetClothes(slot, drawable, texture);
        }
    }
}
