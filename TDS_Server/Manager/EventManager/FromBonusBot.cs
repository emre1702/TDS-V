using BonusBotConnector_Server;
using TDS_Server.Manager.Userpanel;

namespace TDS_Server.Manager.EventManager
{
    class FromBonusBot
    {
        public static void Init()
        {
            BBCommandService.OnUsedCommand += BBCommandService_OnUsedCommand;
        }

        private static void BBCommandService_OnUsedCommand(ulong userId, string command)
        {
            switch (command)
            {
                case "confirmuserid":
                case "confirmidentity":
                case "confirmtds":
                    SettingsNormal.ConfirmDiscordUserId(userId);
                    break;
            }
        }
    }
}
