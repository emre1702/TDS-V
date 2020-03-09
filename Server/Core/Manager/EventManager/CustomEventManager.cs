using GTANetworkAPI;
using TDS_Server.Instance.LobbyInstances;
using TDS_Server.Instance.PlayerInstance;

namespace TDS_Server.Core.Manager.EventManager
{
    static class CustomEventManager
    {
        public delegate void PlayerDelegate(TDSPlayer player);
        public delegate void ClientDelegate(Player client);

        public static event PlayerDelegate? OnPlayerLoggedInBefore;
        public static event PlayerDelegate? OnPlayerLoggedOutBefore;
        public static event PlayerDelegate? OnPlayerLoggedIn;
        public static event PlayerDelegate? OnPlayerLoggedOut;
        public static event ClientDelegate? OnPlayerRegistered;


        public delegate void PlayerLobbyDelegate(TDSPlayer player, Lobby lobby);

        public static event PlayerLobbyDelegate? OnPlayerJoinedLobby;
        /// <summary>
        /// If the player has left the lobby no matter which reason (could also be a disconnect)
        /// </summary>
        public static event PlayerLobbyDelegate? OnPlayerLeftLobby;



        public static void SetPlayerLoggedIn(TDSPlayer player)
        {
            OnPlayerLoggedInBefore?.Invoke(player);
            OnPlayerLoggedIn?.Invoke(player);
        }

        public static void SetPlayerLoggedOut(TDSPlayer player)
        {
            OnPlayerLoggedOutBefore?.Invoke(player);
            OnPlayerLoggedOut?.Invoke(player);
        }

        public static void SetPlayerRegistered(Player client)
        {
            OnPlayerRegistered?.Invoke(client);
        }

        public static void SetPlayerJoinedLobby(TDSPlayer player, Lobby lobby)
        {
            OnPlayerJoinedLobby?.Invoke(player, lobby);
        }

        public static void SetPlayerLeftLobby(TDSPlayer player, Lobby lobby)
        {
            OnPlayerLeftLobby?.Invoke(player, lobby);
        }
    }
}
