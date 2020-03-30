using RAGE;
using RAGE.Game;
using TDS_Client.Enum;
using TDS_Client.Manager.Utility;
using TDS_Shared.Default;
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

        private static void OpenLobbyChatInput(Control _)
        {
            if (Angular.Shared.InInput)
                return;

            OpenChatInput(null);
        }

#pragma warning disable IDE0051 // Remove unused private members
        private static void OpenGlobalChatInput(Control _)
#pragma warning restore IDE0051 // Remove unused private members
        {
            if (Angular.Shared.InInput)
                return;

            OpenChatInput("/globalsay ");
        }

        private static void OpenTeamChatInput(Control _)
        {
            if (Angular.Shared.InInput)
                return;

            OpenChatInput("/teamsay ");
        }

        private static void OpenChatInput(string cmd)
        {
            if (IsOpen)
                return;
            IsOpen = true;

            if (cmd == null)
                Angular.Main.ToggleChatInput(true);
            else
                Angular.Main.ToggleChatInput(true, cmd);
        }

        public static void CloseChatInput(bool force = false)
        {
            if (!IsOpen && !force)
                return;
            IsOpen = false;
            Angular.Main.ToggleChatInput(false);
        }
    }
}
