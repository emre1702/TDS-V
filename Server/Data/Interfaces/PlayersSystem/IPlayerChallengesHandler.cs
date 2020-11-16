using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Shared.Data.Enums.Challenge;

namespace TDS.Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerChallengesHandler
    {
        void AddToChallenge(ChallengeType type, int amount = 1, bool setTheValue = false);

        void Init(ITDSPlayer player, IPlayerEvents events);

        void InitChallengesDict();
    }
}
