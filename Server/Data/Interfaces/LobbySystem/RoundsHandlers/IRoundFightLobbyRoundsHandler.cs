using System.Text;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.Data.Models;

namespace TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers
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
