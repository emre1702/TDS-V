namespace TDS_Client.Data.Interfaces.ModAPI.Player
{
    public interface IPlayerAPI
    {
        void SetPlayerTargetingMode(int v);
        void SetPlayerLockon(bool v);
        bool GetPlayerInvincible();
        void SetPlayerInvincible(bool v);
        void SetPlayerTeam(int team);
        bool GetEntityPlayerIsFreeAimingAt(ref int targetEntity);
    }
}
