using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.CustomAttribute;
using TDS_Server.Data.Defaults;
using TDS_Server.Handler.Extensions;

namespace TDS_Server.Handler.Commands.User
{
    public class UserAccountCommands
    {
        [TDSCommand(UserCommand.UserId)]
        public void OutputUserId(ITDSPlayer player)
        {
            NAPI.Task.RunSafe(() => player.SendChatMessage("User id: " + (player.Entity?.Id.ToString() ?? "?")));
        }
    }
}
