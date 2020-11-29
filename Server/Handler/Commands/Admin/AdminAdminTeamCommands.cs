using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.CustomAttribute;
using TDS.Server.Data.Defaults;

namespace TDS.Server.Handler.Commands.Admin
{
    public class AdminAdminTeamCommands
    {
        [TDSCommand(AdminCommand.SetAdmin, 1)]
        public void SetAdmin(ITDSPlayer player, ITDSPlayer target, int adminLevel)
        {

        }
    }
}
