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

        private static string? BBCommandService_OnUsedCommand(ulong userId, string command)
        {
            switch (command)
            {
                case "confirmtds":
                    return SettingsNormal.ConfirmDiscordUserId(userId);
            }
            return null;
        }
    }
}
