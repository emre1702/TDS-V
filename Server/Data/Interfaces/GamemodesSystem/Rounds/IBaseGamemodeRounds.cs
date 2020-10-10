using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas;
using TDS_Server.Data.Interfaces.TeamsSystem;

namespace TDS_Server.Data.Interfaces.GamemodesSystem.Rounds
{
    public interface IBaseGamemodeRounds
    {
        bool CanEndRound(IRoundEndReason roundEndReason);

        bool CanJoinDuringRound(ITDSPlayer player, ITeam team);
    }
}
