using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerChallengesHandler
    {
        void AddToChallenge(ChallengeType type, int amount = 1, bool setTheValue = false);

        void Init(ITDSPlayer player, IPlayerEvents events);

        void InitChallengesDict();
    }
}
