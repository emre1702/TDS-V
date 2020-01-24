using GTANetworkAPI;
using TDS_Common.Default;
using TDS_Common.Enum.Challenge;
using TDS_Common.Enum.Userpanel;
using TDS_Server.Instance.PlayerInstance;
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
                    json = Commands.GetData();
                    break;

                case EUserpanelLoadDataType.Rules:
                    json = Rules.GetData();
                    player.AddToChallenge(EChallengeType.ReadTheRules);
                    break;

                case EUserpanelLoadDataType.FAQs:
                    json = FAQs.GetData(player);
                    player.AddToChallenge(EChallengeType.ReadTheFAQ);
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

                case EUserpanelLoadDataType.SettingsSpecial:
                    json = SettingsSpecial.GetData(player);
                    break;

                case EUserpanelLoadDataType.SupportUser:
                    json = await SupportUser.GetData(player);
                    break;

                case EUserpanelLoadDataType.SupportAdmin:
                    json = await SupportAdmin.GetData(player);
                    break;
                case EUserpanelLoadDataType.OfflineMessages:
                    json = await OfflineMessages.GetData(player);
                    break;
            }

            if (json == null)
                return;

            NAPI.ClientEvent.TriggerClientEvent(player.Player, DToClientEvent.LoadUserpanelData, (int)dataType, json);
        }

        public static void Init(TDSDbContext dbContext)
        {
            Rules.LoadRules(dbContext);
            FAQs.LoadFAQs(dbContext);
            ApplicationUser.LoadAdminQuestions(dbContext);
        }
    }
}
