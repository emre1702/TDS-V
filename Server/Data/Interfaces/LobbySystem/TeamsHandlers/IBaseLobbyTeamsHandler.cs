using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.TeamsSystem;

namespace TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers
{
#nullable enable

    public interface IBaseLobbyTeamsHandler
    {
        int Count { get; }

        Task Do(Action<ITeam[]> action);

        Task<T> Do<T>(Func<ITeam[], T> func);

        Task<ITeam> GetTeam(short teamIndex);

        List<ITeam> GetTeams();

        Task SetPlayerTeam(ITDSPlayer player, ITeam? team);

        Task<ITeam> GetTeamWithFewestPlayer();

        ITeam GetTeamWithFewestPlayer(ITeam[] teams);
    }
}
