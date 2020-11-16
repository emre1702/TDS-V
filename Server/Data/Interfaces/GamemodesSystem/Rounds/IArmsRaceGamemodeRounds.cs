using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.GamemodesSystem.Rounds
{
    public interface IArmsRaceGamemodeRounds
    {
        bool CheckHasKillerWonTheRound(ITDSPlayer killer);
    }
}
