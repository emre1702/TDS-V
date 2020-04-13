using TDS_Client.Data.Interfaces.ModAPI.Ped;
using TDS_Shared.Data.Enums;

namespace TDS_Client.RAGEAPI.Ped
{
    class PedBase : Entity.EntityBase, IPedBase
    {
        private readonly RAGE.Elements.PedBase _instance;

        protected PedBase(RAGE.Elements.PedBase instance) : base(instance)
            => _instance = instance;

        public bool IsClimbing => _instance.IsClimbing();
        public int Armor 
        { 
            get => _instance.GetArmour(); 
            set => _instance.SetArmour(value); 
        }

        public void ClearBloodDamage()
        {
            _instance.ClearBloodDamage();
        }

        public void ClearLastDamageBone()
        {
            _instance.ClearLastDamageBone();
        }

        public void ClearLastWeaponDamage()
        {
            _instance.ClearLastWeaponDamage();
        }

        public void ExplodeHead(WeaponHash weaponHash)
        {
            _instance.ExplodeHead((uint)weaponHash);
        }

        public void GetAmmoInClip(WeaponHash weaponHash, ref int ammoInClip)
        {
            _instance.GetAmmoInClip((uint)weaponHash, ref ammoInClip);
        }

        public int GetAmmoInWeapon(WeaponHash currentWeapon)
        {
            int ammo = 0;
            _instance.GetAmmoInClip((uint)currentWeapon, ref ammo);
            return ammo;
        }

        public WeaponHash GetSelectedWeapon()
        {
            return (WeaponHash)_instance.GetSelectedWeapon();
        }

        public bool IsDeadOrDying()
        {
            return _instance.IsDeadOrDying(true);
        }

        public void ResetMovementClipset(float clipSetSwitchTime)
        {
            _instance.ResetMovementClipset(clipSetSwitchTime);
        }

        public void ResetStrafeClipset()
        {
            _instance.ResetStrafeClipset();
        }

        public void ResetVisibleDamage()
        {
            _instance.ResetVisibleDamage();
        }

        public void SetCanAttackFriendly(bool toggle)
        {
            _instance.SetCanAttackFriendly(toggle, false);
        }

        public void SetMovementClipset(string clipSet, float clipSetSwitchTime)
        {
            _instance.SetMovementClipset(clipSet, clipSetSwitchTime);
        }

        public void SetStrafeClipset(string clipSet)
        {
            _instance.SetStrafeClipset(clipSet);
        }

        public void TaskPlayAnim(string animDict, string animName, float speed, int speedMultiplier, int duration, int flat, int playbackRate, bool lockX, bool lockY, bool lockZ)
        {
            _instance.TaskPlayAnim(animDict, animName, speed, speedMultiplier, duration, flat, playbackRate, lockX, lockY, lockZ);
        }
    }
}
