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
            Events.CallRemote(DToServerEvent.ChatLoaded, Settings.Language, Settings.HitsoundOn);
        }

        public static void CommandUsed(string msg)
        {
            string[] args = msg.Split(' ');
            Events.CallRemote(DToServerEvent.CommandUsed, args[0], args.Skip(1).ToArray());
        }
    }
}
