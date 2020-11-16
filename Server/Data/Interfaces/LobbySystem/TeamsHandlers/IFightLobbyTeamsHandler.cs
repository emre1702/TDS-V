using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Data.Interfaces.LobbySystem.TeamsHandlers
{
#nullable enable

    public interface IFightLobbyTeamsHandler : IBaseLobbyTeamsHandler
    {
        Task<ITeam> GetNextNonSpectatorTeam(ITeam start);

        Task<ITeam> GetNextNonSpectatorTeam(short startIndex);

        Task<ITeam?> GetNextTeamWithSpectatablePlayers(ITeam? start);

        Task<ITeam?> GetNextTeamWithSpectatablePlayers(short startIndex);

        Task<ITeam?> GetPreviousTeamWithSpectatablePlayers(ITeam? start);

        Task<ITeam?> GetPreviousNonSpectatorTeamWithPlayers(short startIndex);

        void SendTeamOrder(ITDSPlayer player, TeamOrder teamOrder);

        Task<string> GetAmountInFightSyncDataJson();
    }
}
