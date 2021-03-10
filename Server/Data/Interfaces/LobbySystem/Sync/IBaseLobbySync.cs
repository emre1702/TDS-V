namespace TDS.Server.Data.Interfaces.LobbySystem.Sync
{
    public interface IBaseLobbySync
    {
        public bool IsLobbyToSync { get; }

        void TriggerEvent(string eventName, params object[] args);
    }
}