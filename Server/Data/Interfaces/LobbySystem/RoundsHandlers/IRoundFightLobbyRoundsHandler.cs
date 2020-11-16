using System.Text;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Data.Models;

namespace TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers
{
#nullable enable

    public interface IRoundFightLobbyRoundsHandler
    {
        IBaseGamemode? CurrentGamemode { get; }
        IRoundStatesHandler RoundStates { get; }

        void RewardPlayerForRound(ITDSPlayer player, RoundPlayerRewardsData rewardsData);

        void AddRoundRewardsMessage(ITDSPlayer player, StringBuilder useStringBuilder, RoundPlayerRewardsData toModel);

        ValueTask<ITeam?> GetTimesUpWinnerTeam();

        void SetPlayerReadyForRound(ITDSPlayer player, bool freeze);

        void StartRoundForPlayer(ITDSPlayer player);

        Task CheckForEnoughAlive();
    }
}
