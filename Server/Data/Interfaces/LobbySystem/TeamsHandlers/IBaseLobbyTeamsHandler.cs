using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.TeamsSystem;

namespace TDS.Server.Data.Interfaces.LobbySystem.TeamsHandlers
{
#nullable enable

    public interface IBaseLobbyTeamsHandler
    {
        int Count { get; }

        Task DoForList(Action<ITeam[]> action);

        Task<T> DoForList<T>(Func<ITeam[], T> func);

        Task<ITeam> GetTeam(short teamIndex);

        List<ITeam> GetTeams();

        void SetPlayerTeam(ITDSPlayer player, ITeam? team);

        Task<ITeam> GetTeamWithFewestPlayer();

        ITeam GetTeamWithFewestPlayer(ITeam[] teams);
    }
}
