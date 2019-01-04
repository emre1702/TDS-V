using RAGE;
using System.Linq;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using static RAGE.Events;

namespace TDS_Client.Manager.Browser
{
    class ChatManager : Script
    {
        public static bool IsOpen { get; set; }

        public ChatManager()
        {
            Chat.Show(false);
        }

        public static void Loaded()
        {
            //Settings.LoadSettings();
            MainBrowser.LoadUserName();
            CallRemote(DToServerEvent.ChatLoaded);
        }

        public static void CommandUsed(string msg)
        {
            CallRemote(DToServerEvent.CommandUsed, msg);
        }
    }
}
