using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers
{
    public interface IBaseLobbyTeamsHandler
    {
        Task Do(Action<List<ITeam>> action);
        Task<T> Do<T>(Func<List<ITeam>, T> func);
        Task<ITeam> GetTeam(short teamIndex);
        List<ITeam> GetTeams();
        void SetPlayerTeam(ITDSPlayer player, ITeam? team);
    }
}