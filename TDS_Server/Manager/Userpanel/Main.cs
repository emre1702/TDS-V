using GTANetworkAPI;
using TDS_Common.Default;
using TDS_Common.Enum.Userpanel;
using TDS_Server.Instance.Player;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Userpanel
{
    class Main
    {
        public static async void PlayerLoadData(TDSPlayer player, EUserpanelLoadDataType dataType)
        {
            string? json = null;
            switch (dataType)
            {
                case EUserpanelLoadDataType.Commands:
                    json = Commands.GetData(player);
                    break;

                case EUserpanelLoadDataType.Rules:
                    json = Rules.GetData(player);
                    break;

                case EUserpanelLoadDataType.FAQs:
                    json = FAQs.GetData(player);
                    break;

                case EUserpanelLoadDataType.MyStats:
                    json = await PlayerStats.GetData(player);
                    break;

                case EUserpanelLoadDataType.ApplicationUser:
                    json = await ApplicationUser.GetData(player);
                    break;

                case EUserpanelLoadDataType.ApplicationsAdmin:
                    json = await ApplicationsAdmin.GetData(player);
                    break;
            }

            if (json == null)
                return;

            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.LoadUserpanelData, (int)dataType, json);
        }

        public static void Init(TDSNewContext dbContext)
        {
            Rules.LoadRules(dbContext);
            FAQs.LoadFAQs(dbContext);
            ApplicationUser.LoadAdminQuestions(dbContext);
        }
    }
}
