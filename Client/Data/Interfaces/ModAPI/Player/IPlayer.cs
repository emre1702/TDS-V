using TDS_Client.Data.Interfaces.ModAPI.Ped;
using TDS_Shared.Data.Enums;

namespace TDS_Client.Data.Interfaces.ModAPI.Player
{
    public interface IPlayer : IPedBase
    {
        string Name { get; }
        ushort RemoteId { get; }
        object AutoVolume { get; set; }
        float VoiceVolume { get; set; }
        object Voice3d { get; set; }
        bool IsPlaying { get; }
        bool IsClimbing { get; }
        bool IsFreeAiming { get; }
        float Heading { get; set; }
        int Armor { get; set; }
        int Health { get; set; }
        bool Exists { get; }

        void SetMaxArmor(int maxArmor);
        void TaskPlayAnim(string v1, string animName, float v2, int v3, int v4, int v5, int v6, bool v7, bool v8, bool v9);
        bool IsPlayingAnim(string v1, string animName, int v2);
        bool HasAnimEventFired(object p);
        void SetVisible(bool v1, bool v2);
        float GetAnimCurrentTime(string v, string animName);
        WeaponHash GetSelectedWeapon();
        void GetAmmoInClip(WeaponHash currentWeapon, ref int ammoInClip);
        int GetAmmoInWeapon(WeaponHash currentWeapon);
        void ClearLastDamageBone();
        void ClearLastDamageEntity();
        void ClearLastWeaponDamage();
        void SetMovementClipset(string movementClipSet, float clipSetSwitchTime);
        void SetStrafeClipset(string strafeClipSet);
        void ResetVisibleDamage();
        void ClearBloodDamage();
        void ResetMovementClipset(float clipSetSwitchTime);
        void ResetStrafeClipset();
        void DisablePlayerFiring(bool v);
        // RAGE.Game.Player.SetPlayerMaxArmour(Constants.MaxPossibleArmor);
    }
}
