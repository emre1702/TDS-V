using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.CustomAttribute;
using TDS.Server.Data.Defaults;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.Handler.Commands.User
{
    public class UserMapCommands
    {
        [TDSCommandAttribute(UserCommand.Position)]
        public void OutputCurrentPosition(ITDSPlayer player)
        {
            NAPI.Task.RunSafe(() =>
            {
                if (player.IsInVehicle && player.Vehicle is { })
                {
                    var pos = player.Vehicle.Position;
                    player.SendChatMessage("Vehicle X: " + pos.X + " Y: " + pos.Y + " Z: " + pos.Z);
                    var rot = player.Vehicle.Rotation;
                    player.SendChatMessage("Vehicle ROT RX: " + rot.X + " RY: " + rot.Y + " RZ: " + rot.Z);
                    player.SendChatMessage($"Vehicle dimension: {player.Vehicle.Dimension} | Your dimension: {player.Dimension}");
                }
                else
                {
                    var pos = player.Position;
                    player.SendChatMessage("Player X: " + pos.X + " Y: " + pos.Y + " Z: " + pos.Z);
                    var rot = player.Rotation;
                    player.SendChatMessage("Player ROT: " + rot);
                    player.SendChatMessage($"Dimension: {player.Dimension}");
                }
            });
        }
    }
}
