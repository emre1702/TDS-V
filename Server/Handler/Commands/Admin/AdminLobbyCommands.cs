using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.CustomAttribute;
using TDS.Server.Data.Defaults;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Models;
using TDS.Server.Data.RoundEndReasons;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Handler.Commands.Admin
{
    public class AdminLobbyCommands
    {
        [TDSCommand(AdminCommand.NextMap)]
        public void NextMap(ITDSPlayer player, TDSCommandInfos cmdinfos, [RemainingText(MinLength = 4)] string reason)
        {
            if (!(player.Lobby is IArena arena))
                return;
            if (!cmdinfos.AsLobbyOwner)
                LoggingHandler.Instance.LogAdmin(LogType.Next, player, reason, asdonator: cmdinfos.AsDonator, asvip: cmdinfos.AsVIP);

            arena.Rounds.RoundStates.EndRound(new CommandRoundEndReason(player));
        }
    }
}
