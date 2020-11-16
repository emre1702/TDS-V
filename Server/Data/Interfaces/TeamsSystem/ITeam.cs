using System;
using TDS.Server.Database.Entity.Rest;
using TDS.Shared.Data.Models;

namespace TDS.Server.Data.Interfaces.TeamsSystem
{
#nullable enable

    public interface ITeam : IEquatable<ITeam>
    {
        ITeamChat Chat { get; }
        ITeamSync Sync { get; }
        ITeamPlayers Players { get; }

        Teams Entity { get; }
        bool IsSpectator { get; }
        int SpawnCounter { get; set; }
        SyncedTeamDataDto SyncedData { get; set; }

        void Init(Teams entity);
    }
}
