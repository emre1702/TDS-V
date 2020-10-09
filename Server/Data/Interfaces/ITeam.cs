using GTANetworkAPI;
using System;
using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Database.Entity.Rest;
using TDS_Shared.Data.Models;

namespace TDS_Server.Data.Interfaces
{
#nullable enable

    public interface ITeam : IEquatable<ITeam>
    {
        List<ITDSPlayer>? AlivePlayers { get; }
        string ChatColor { get; }
        Teams Entity { get; }
        bool IsSpectator { get; }
        List<ITDSPlayer> Players { get; }
        int SpawnCounter { get; set; }
        List<ITDSPlayer>? SpectateablePlayers { get; }
        SyncedTeamDataDto SyncedTeamData { get; set; }

        void AddPlayer(ITDSPlayer tdsPlayer);

        void FuncIterate(Action<ITDSPlayer> func);

        ITDSPlayer? GetNearestPlayer(Vector3 position);

        void RemoveAlivePlayer(ITDSPlayer player);

        void RemovePlayer(ITDSPlayer tdsPlayer);

        void SyncAddedPlayer(ITDSPlayer player);

        void SyncAllPlayers();

        void SyncRemovedPlayer(ITDSPlayer player);

        void SendMessage(string message);
    }
}
