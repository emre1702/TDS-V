using System;
using System.Collections.Generic;
using TDS_Server.Database.Entity.Rest;
using TDS_Shared.Data.Models;

namespace TDS_Server.Data.Interfaces
{
    #nullable enable
    public interface ITeam : IEquatable<ITeam>
    {
        Teams Entity { get; }
        List<ITDSPlayer> Players { get; }
        string ChatColor { get; }
        bool IsSpectator { get; }
        List<ITDSPlayer>? SpectateablePlayers { get; }
        int SpawnCounter { get; set; }
        List<ITDSPlayer>? AlivePlayers { get; }
        SyncedTeamDataDto SyncedTeamData { get; set; }

        void AddPlayer(ITDSPlayer tdsPlayer);
        void RemovePlayer(ITDSPlayer tdsPlayer);
        void FuncIterate(Action<ITDSPlayer, ITeam> func);
        void RemoveAlivePlayer(ITDSPlayer player);
        void SyncRemovedPlayer(ITDSPlayer player);
        void SyncAddedPlayer(ITDSPlayer player);
        void SyncAllPlayers();
    }
}
