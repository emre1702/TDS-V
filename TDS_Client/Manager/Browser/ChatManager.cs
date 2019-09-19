using RAGE;
using RAGE.Game;
using TDS_Client.Enum;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using Script = RAGE.Events.Script;

namespace TDS_Client.Manager.Browser
{
    internal class ChatManager : Script
    {
        public static bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (_isOpen == value)
                    return;
                _isOpen = value;
                Browser.Angular.Main.ToggleChatOpened(value);
                if (value)
                    TickManager.Add(OnUpdate);
                else
                    TickManager.Remove(OnUpdate);
            }
        }

        private static bool _isOpen;

        public ChatManager()
        {
            Chat.Show(false);
            BindManager.Add(Control.MpTextChatAll, OpenLobbyChatInput);
            BindManager.Add(Control.MpTextChatTeam, OpenTeamChatInput);
            BindManager.Add(EKey.Escape, (_) => CloseChatInput());
        }

        public static void OnUpdate()
        {
            Pad.DisableAllControlActions((int)EInputGroup.LOOK);
            Pad.DisableAllControlActions((int)EInputGroup.MOVE);
            Pad.DisableAllControlActions((int)EInputGroup.SUB);
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

        public static void CloseChatInput(bool force = false)
        {
            if (!IsOpen && !force)
                return;
            IsOpen = false;
            MainBrowser.Browser.ExecuteJs($"enableChatInput(0);");
        }
    }
}