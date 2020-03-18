using System;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.ModAPI.Player
{
    public interface IPlayer : IEntity, IEquatable<IPlayer>
    {
        bool Dead { get; }
        string IPAddress { get; }
        string Name { get; set; }
        Position3D Position { get; set; }
        ushort RemoteId { get; }

        void SetInvincible(bool v);

        string Serial { get; }
        ulong SocialClubId { get; }
        string SocialClubName { get; }
        int Transparency { get; set; }
        int VehicleSeat { get; }

        void RemoveAllWeapons();

        WeaponHash CurrentWeapon { get; set; }

        void SetCollisionless(bool v, ILobby lobby);

        uint Dimension { get; set; }
        int Armor { get; set; }
        int Health { get; set; }
        float Rotation { get; set; }
        bool IsInVehicle { get; }

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
        void PlayAnimation(string v1, string v2, int loop);
        void Freeze(bool v);
        void SendMessage(string msg);
        void SendNotification(string msg, bool flashing);
        void WarpOutOfVehicle();
        void SetIntoVehicle(IVehicle vehicle, int v);
    }
}
