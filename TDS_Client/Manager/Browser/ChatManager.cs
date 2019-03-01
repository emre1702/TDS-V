using RAGE;
using RAGE.Game;
using System.Linq;
using TDS_Client.Default;
using TDS_Client.Enum;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using static RAGE.Events;
using Script = RAGE.Events.Script;

namespace TDS_Client.Manager.Browser
{
    class ChatManager : Script
    {
        public static bool IsOpen { get; set; }

        public ChatManager()
        {
            Chat.Show(false);
            BindManager.Add(Control.MpTextChatAll, OpenLobbyChatInput);
            BindManager.Add(Control.MpTextChatTeam, OpenTeamChatInput);
        }

        public static void OnUpdate()
        {
            if (IsOpen)
            {
                Pad.DisableControlAction(1, 199, true);
                Pad.DisableControlAction(1, 200, true);
            }
        }

        public static void Loaded()
        {
            //Settings.LoadSettings();
            MainBrowser.LoadUserName();
            EventsSender.Send(DToServerEvent.ChatLoaded);
        }

        private static void OpenLobbyChatInput(Control _)
        {
            OpenChatInput(null);
        }


        private static void OpenGlobalChatInput(Control _)
        {
            OpenChatInput("/globalsay ");

        }

        private static void OpenTeamChatInput(Control _)
        {
            OpenChatInput("/teamsay ");
        }

        private static void OpenChatInput(string cmd)
        {
            if (IsOpen)
                return;
            IsOpen = true;

            if (cmd == null)
                MainBrowser.Browser.ExecuteJs($"enableChatInput(1);");
            else
                MainBrowser.Browser.ExecuteJs($"enableChatInput(1, '{cmd}')");
        }

        public static void CloseChatInput()
        {
            IsOpen = false;
            MainBrowser.Browser.ExecuteJs($"enableChatInput(0);");
        }
    }
}
