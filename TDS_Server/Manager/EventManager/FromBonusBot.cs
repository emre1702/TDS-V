using BonusBotConnector_Server;

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
                case "confirmidentity":

                    break;
            }
        }
    }
}
