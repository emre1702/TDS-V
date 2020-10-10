using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.GamemodesSystem.Rounds
{
    public interface IArmsRaceGamemodeRounds
    {
        bool CheckHasKillerWonTheRound(ITDSPlayer killer);
    }
}
