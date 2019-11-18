
using TDS_Client.Manager.Utility;
using TDS_Common.Dto;

namespace TDS_Client.Manager.Event
{
    static class CustomEventManager
    {
        public delegate void LobbySwitchDelegate(SyncedLobbySettingsDto settings);

        public static event LobbySwitchDelegate OnLobbyLeave;
        public static event LobbySwitchDelegate OnLobbyJoin;

        public static void SetLobbyLeave(SyncedLobbySettingsDto settings)
        {
            //OnPlayerLoggedInBefore?.Invoke(player);
            OnLobbyLeave?.Invoke(settings);
        }

        public static void SetLobbyJoin(SyncedLobbySettingsDto settings)
        {
            //OnPlayerLoggedOutBefore?.Invoke(player);
            OnLobbyJoin?.Invoke(settings);
        }
    }
}
