using System;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.ModAPI.Player
{
    public interface IPlayer : IEntity
    {
        bool Dead { get; }
        string IPAddress { get; }
        string Name { get; set; }
        ushort RemoteId { get; }

        void SetInvincible(bool v);

        string Serial { get; }
        ulong SocialClubId { get; }
        string SocialClubName { get; }
        int Transparency { get; set; }
        int VehicleSeat { get; }

        void RemoveAllWeapons();

        WeaponHash CurrentWeapon { get; set; }

        int Armor { get; set; }
        int Health { get; set; }
        bool IsInVehicle { get; }
        IVehicle Vehicle { get; }
        bool IsDead { get; }

        void Kick(string reason);
        void SendEvent(string eventName, params object[] args);
        void SetHealth(int health);
        void SetSkin(PedHash pedHash);
        void SetVoiceTo(ITDSPlayer target, bool on);
        void Spawn(Position3D position, float heading = 0);
        void GiveWeapon(WeaponHash hash, int ammo = 0);
        void SetWeaponAmmo(WeaponHash hash, int ammo);
        void StopAnimation();
        void Kill();
        void PlayAnimation(string animDict, string animName, int flag);
        void Freeze(bool toggle);
        void SendMessage(string msg);
        void SendNotification(string msg, bool flashing = false);
        void WarpOutOfVehicle();
        void SetIntoVehicle(IVehicle vehicle, int seat);
        void SetClothes(int slot, int drawable, int texture);
    }
}
