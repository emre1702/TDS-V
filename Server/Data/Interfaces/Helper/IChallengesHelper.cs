using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Database.Entity.Challenge;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Data.Interfaces.Helper
{
#nullable enable

    public interface IChallengesHelper
    {
        Task AddForeverChallenges(ITDSPlayer player, Players dbPlayer);

        Task AddWeeklyChallenges(ITDSPlayer player);

        void ClearWeeklyChallenges();

        string GetChallengesJson(ITDSPlayer player);

        void SyncCurrentAmount(ITDSPlayer player, PlayerChallenges challenge);
    }
}