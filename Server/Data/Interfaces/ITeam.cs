using System;
using System.Collections.Generic;
using TDS_Server.Database.Entity.Rest;
using TDS_Shared.Data.Models;

namespace TDS_Server.Data.Interfaces
{
#nullable enable

    public interface ITeam : IEquatable<ITeam>
    {
        #region Public Properties

        List<ITDSPlayer>? AlivePlayers { get; }
        string ChatColor { get; }
        Teams Entity { get; }
        bool IsSpectator { get; }
        List<ITDSPlayer> Players { get; }
        int SpawnCounter { get; set; }
        List<ITDSPlayer>? SpectateablePlayers { get; }
        SyncedTeamDataDto SyncedTeamData { get; set; }

        #endregion Public Properties

        #region Public Methods

        void AddPlayer(ITDSPlayer tdsPlayer);

        void FuncIterate(Action<ITDSPlayer, ITeam> func);

        void RemoveAlivePlayer(ITDSPlayer player);

        void RemovePlayer(ITDSPlayer tdsPlayer);

        void SyncAddedPlayer(ITDSPlayer player);

        void SyncAllPlayers();

        void SyncRemovedPlayer(ITDSPlayer player);

        #endregion Public Methods
    }
}
