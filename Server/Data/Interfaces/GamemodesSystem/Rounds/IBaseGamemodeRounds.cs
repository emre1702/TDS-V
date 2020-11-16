using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas;
using TDS.Server.Data.Interfaces.TeamsSystem;

namespace TDS.Server.Data.Interfaces.GamemodesSystem.Rounds
{
    public interface IBaseGamemodeRounds
    {
        bool CanEndRound(IRoundEndReason roundEndReason);

        bool CanJoinDuringRound(ITDSPlayer player, ITeam team);
    }
}
