using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers
{
#nullable enable

    public interface IBaseLobbyTeamsHandler
    {
        Task Do(Action<List<ITeam>> action);

        Task<T> Do<T>(Func<List<ITeam>, T> func);

        Task<ITeam> GetTeam(short teamIndex);

        List<ITeam> GetTeams();

        Task SetPlayerTeam(ITDSPlayer player, ITeam? team);

        Task<ITeam> GetTeamWithFewestPlayer();

        ITeam GetTeamWithFewestPlayer(List<ITeam> teams);
    }
}
