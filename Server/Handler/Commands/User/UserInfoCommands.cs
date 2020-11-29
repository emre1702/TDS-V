using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.CustomAttribute;
using TDS.Server.Data.Defaults;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.Handler.Commands.User
{
    public class UserInfoCommands
    {
        private readonly AdminsHandler _adminsHandler;

        public UserInfoCommands(AdminsHandler adminsHandler) 
            => _adminsHandler = adminsHandler;

        [TDSCommand(UserCommand.Admins)]
        public void OutputOnlineAdmins(ITDSPlayer player)
        {
            var admins = _adminsHandler.GetAllAdminsSorted();

            NAPI.Task.RunSafe(() =>
            {
                var lastAdminLevel = -1;
                foreach (var admin in admins)
                {
                    if (lastAdminLevel != admin.Admin.Level.Level)
                    {
                        lastAdminLevel = admin.Admin.Level.Level;
                        player.SendChatMessage(admin.Admin.Level.FontColor + admin.Admin.LevelName);
                    }
                    player.SendChatMessage(admin.DisplayName);
                }
            });
        }

        [TDSCommand(UserCommand.UserId)]
        public void OutputUserId(ITDSPlayer player)
        {
            NAPI.Task.RunSafe(() => player.SendChatMessage("User id: " + (player.Entity?.Id.ToString() ?? "?")));
        }
    }
}
