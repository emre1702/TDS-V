namespace TDS.Server.Data.Interfaces.LobbySystem.Sync
{
    public interface IBaseLobbySync
    {
        void TriggerEvent(string eventName, params object[] args);
    }
}