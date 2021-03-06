﻿using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.TeamsSystem
{
    public interface ITeamSync
    {
        bool SyncChanges { get; set; }

        void Init(ITeam team);

        void SyncAddedPlayer(ITDSPlayer player);

        void SyncAllPlayers();

        void SyncRemovedPlayer(ITDSPlayer player);

        void TriggerEvent(string eventName, params object[] args);

        void TriggerEventExcept(ITDSPlayer exceptPlayer, string eventName, params object[] args);
    }
}
