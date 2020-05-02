using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Ped
{
    public interface IPedBase : IEntityBase
    {
        bool IsClimbing { get; }
        int Armor { get; set; }
        void ClearBloodDamage();
        void ClearLastDamageBone();
        new void ClearLastWeaponDamage();
        void ExplodeHead(WeaponHash weaponHash);
        int GetAmmoInClip(WeaponHash weaponHash);
        int GetAmmoInWeapon(WeaponHash weaponHash);
        WeaponHash GetSelectedWeapon();
        bool IsDeadOrDying();
        void ResetMovementClipset(float clipSetSwitchTime);
        void ResetStrafeClipset();
        void ResetVisibleDamage();
        void SetCanAttackFriendly(bool toggle);
        void SetMovementClipset(string clipSet, float clipSetSwitchTime);
        void SetStrafeClipset(string clipSet);
        void TaskPlayAnim(string animDict, string animName, float speed, int speedMultiplier, int duration, int flat, int playbackRate, bool lockX, bool lockY, bool lockZ);
    }
}
