using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
