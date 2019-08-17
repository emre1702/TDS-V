using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Userpanel
{
    class Main
    {
        public static void PlayerLoadData(TDSPlayer player, EUserpanelLoadDataType dataType)
        {
            switch (dataType)
            {
                case EUserpanelLoadDataType.Commands:
                    Commands.SendPlayerCommandData(player);
                    break;

                case EUserpanelLoadDataType.Rules:
                    Rules.SendPlayerRules(player);
                    break;

                case EUserpanelLoadDataType.FAQs:
                    FAQs.SendPlayerFAQs(player);
                    break;
            }
        }

        public static void Init(TDSNewContext dbContext)
        {
            Rules.LoadRules(dbContext);
            FAQs.LoadFAQs(dbContext);
        }
    }
}
