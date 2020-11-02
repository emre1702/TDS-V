using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.CustomAttribute;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Models;
using TDS_Server.Data.Utility;
using TDS_Server.Handler.Extensions;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Commands.Admin
{
    public class AdminMapCommands
    {
        [TDSCommand(AdminCommand.Goto)]
        public void GotoPlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, ITDSPlayer target, [TDSRemainingText(MinLength = 4)] string reason)
        {
            NAPI.Task.RunSafe(() =>
            {
                var targetPos = target.Position;

                if (player.IsInVehicle && player.Vehicle is { })
                {
                    var pos = targetPos.Around(2f, false);
                    player.Vehicle.Position = pos;
                    return;
                }

                if (target.IsInVehicle && target.Vehicle is { })
                {
                    uint? freeSeat = Utils.GetVehicleFreeSeat(target.Vehicle);
                    if (freeSeat.HasValue)
                    {
                        player.SetIntoVehicle(target.Vehicle, (int)freeSeat.Value);
                        return;
                    }
                }

                player.Position = targetPos.Around(2f, false);
            });

            if (!cmdinfos.AsLobbyOwner)
                LoggingHandler.Instance.LogAdmin(LogType.Goto, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.Goto)]
        public void GotoVector(ITDSPlayer player, TDSCommandInfos cmdinfos, Vector3 pos, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (player is null)
                return;

            NAPI.Task.RunSafe(() => player.Position = pos);

            if (!cmdinfos.AsLobbyOwner)
                LoggingHandler.Instance.LogAdmin(LogType.Goto, player, null, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }
    }
}
