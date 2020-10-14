﻿using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.TeamsSystem
{
    public class Sync : ITeamSync
    {
        public bool SyncChanges { get; set; } = true;

#nullable disable
        private ITeam _team;
#nullable enable

        public void Init(ITeam team)
        {
            _team = team;
        }

        public void TriggerEvent(string eventName, params object[] args)
        {
            var playersToSyncTo = _team.Players.GetAllArray();
            NAPI.Task.Run(() =>
                NAPI.ClientEvent.TriggerClientEventToPlayers(playersToSyncTo, eventName, args));
        }

        public void TriggerEventExcept(ITDSPlayer exceptPlayer, string eventName, params object[] args)
        {
            var playersToSyncTo = _team.Players.GetAllArrayExcept(exceptPlayer);
            NAPI.Task.Run(() =>
                NAPI.ClientEvent.TriggerClientEventToPlayers(playersToSyncTo, eventName, args));
        }

        public void SyncAddedPlayer(ITDSPlayer player)
        {
            if (!SyncChanges)
                return;
            string _allPlayersRemoteIdsJson = Serializer.ToClient(_team.Players.GetRemoteIds());
            var playersToSyncTo = _team.Players.GetAllArrayExcept(player);
            NAPI.Task.Run(() =>
            {
                player.TriggerEvent(ToClientEvent.SyncTeamPlayers, _allPlayersRemoteIdsJson);
                NAPI.ClientEvent.TriggerClientEventToPlayers(playersToSyncTo, ToClientEvent.PlayerJoinedTeam, player.RemoteId);

                foreach (var target in playersToSyncTo)
                {
                    if (!player.HasRelationTo(target, PlayerRelation.Block) && !target.IsVoiceMuted)
                        target.SetVoiceTo(player, true);
                    if (!target.HasRelationTo(player, PlayerRelation.Block) && !player.IsVoiceMuted)
                        player.SetVoiceTo(target, true);
                }
            });
        }

        public void SyncAllPlayers()
        {
            if (!SyncChanges)
                return;
            string _allPlayersRemoteIdsJson = Serializer.ToClient(_team.Players.GetRemoteIds());
            var playersToSyncTo = _team.Players.GetAllArray();
            NAPI.Task.Run(() =>
            {
                NAPI.ClientEvent.TriggerClientEventToPlayers(playersToSyncTo, ToClientEvent.SyncTeamPlayers, _allPlayersRemoteIdsJson);
                foreach (var player in playersToSyncTo)
                    foreach (var target in playersToSyncTo)
                    {
                        if (target == player)
                            continue;
                        if (!player.HasRelationTo(target, PlayerRelation.Block) && !target.IsVoiceMuted)
                            target.SetVoiceTo(player, true);
                        if (!target.HasRelationTo(player, PlayerRelation.Block) && !player.IsVoiceMuted)
                            player.SetVoiceTo(target, true);
                    }
            });
        }

        public void SyncRemovedPlayer(ITDSPlayer player)
        {
            if (!SyncChanges)
                return;
            var playersToSyncTo = _team.Players.GetAllArrayExcept(player);
            NAPI.Task.Run(() =>
            {
                player.ResetVoiceToAndFrom();

                NAPI.ClientEvent.TriggerClientEventToPlayers(playersToSyncTo, ToClientEvent.PlayerLeftTeam, player.RemoteId);
                player.TriggerEvent(ToClientEvent.ClearTeamPlayers);
            });
        }
    }
}
