using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Instance.Lobby;
using TDS_Server.Instance.Player;

namespace TDS_Server.Manager.Utility
{
    static class CustomEventManager
    {
        public delegate void PlayerDelegate(TDSPlayer player);
        public delegate void ClientDelegate(Client client);

        public static event PlayerDelegate? OnPlayerLoggedInBefore;
        public static event PlayerDelegate? OnPlayerLoggedOutBefore;
        public static event PlayerDelegate? OnPlayerLoggedIn;
        public static event PlayerDelegate? OnPlayerLoggedOut;
        public static event ClientDelegate? OnPlayerRegistered;


        public delegate void PlayerLobbyDelegate(TDSPlayer player, Lobby lobby);

        public static event PlayerLobbyDelegate? OnPlayerJoinedLobby;
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

        public static void SetPlayerRegistered(Client client)
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
