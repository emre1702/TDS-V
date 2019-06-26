using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Instance.Player;

namespace TDS_Server.Manager.Utility
{
    static class CustomEventManager
    {
        public delegate void PlayerLoggedInOutDelegate(TDSPlayer player);
        public delegate void PlayerLoggedOutDelegate(TDSPlayer player);

        public static event PlayerLoggedInOutDelegate OnPlayerLoggedInBefore;
        public static event PlayerLoggedInOutDelegate OnPlayerLoggedOutBefore;
        public static event PlayerLoggedInOutDelegate OnPlayerLoggedIn;
        public static event PlayerLoggedInOutDelegate OnPlayerLoggedOut;

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
    }
}
