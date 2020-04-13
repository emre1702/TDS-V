namespace TDS_Client.Data.Interfaces.ModAPI.Player
{
    public interface IPlayerAPI
    {
        void SetPlayerTargetingMode(int targetingMode);
        void SetPlayerLockon(bool toggle);
        bool GetPlayerInvincible();
        void SetPlayerInvincible(bool toggle);
        void SetPlayerTeam(int team);
        bool GetEntityPlayerIsFreeAimingAt(ref int targetEntity);
        void SetPlayerHealthRechargeMultiplier(float regenRate);
    }
}
