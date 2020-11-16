using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.CustomAttribute;
using TDS.Server.Data.Defaults;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.Handler.Commands.User
{
    public class UserAccountCommands
    {
        [TDSCommandAttribute(UserCommand.UserId)]
        public void OutputUserId(ITDSPlayer player)
        {
            NAPI.Task.RunSafe(() => player.SendChatMessage("User id: " + (player.Entity?.Id.ToString() ?? "?")));
        }
    }
}
