using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.CustomAttribute;
using TDS.Server.Data.Defaults;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.Handler.Commands.Admin
{
    public class AdminTestCommands
    {
        [TDSCommandAttribute(AdminCommand.Test)]
        public void Test(ITDSPlayer player, string type, string? arg1 = null, string? arg2 = null, string? arg3 = null)
        {
            switch (type)
            {
                case "cloth":
                    if (player is null)
                        return;

                    if (!int.TryParse(arg1, out int slot))
                    {
                        NAPI.Task.RunSafe(() => player.SendNotification(player.Language.COMMAND_USED_WRONG, true));
                        return;
                    }
                    if (!int.TryParse(arg2, out int drawable))
                    {
                        NAPI.Task.RunSafe(() => player.SendNotification(player.Language.COMMAND_USED_WRONG, true));
                        return;
                    }
                    if (!int.TryParse(arg3, out int texture))
                    {
                        NAPI.Task.RunSafe(() => player.SendNotification(player.Language.COMMAND_USED_WRONG, true));
                        return;
                    }

                    NAPI.Task.RunSafe(() => player.SetClothes(slot, drawable, texture));
                    break;

                default:
                    NAPI.Task.RunSafe(() => player.SendNotification(player.Language.COMMAND_DOESNT_EXIST, true));
                    break;
            }
        }
    }
}
