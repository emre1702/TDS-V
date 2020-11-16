using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;
using TDS.Shared.Default;

namespace TDS.Server.TeamsSystem
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
            NAPI.Task.RunSafe(() =>
                NAPI.ClientEvent.TriggerClientEventToPlayers(playersToSyncTo, eventName, args));
        }

        public void TriggerEventExcept(ITDSPlayer exceptPlayer, string eventName, params object[] args)
        {
            var playersToSyncTo = _team.Players.GetAllArrayExcept(exceptPlayer);
            NAPI.Task.RunSafe(() =>
                NAPI.ClientEvent.TriggerClientEventToPlayers(playersToSyncTo, eventName, args));
        }

        public void SyncAddedPlayer(ITDSPlayer player)
        {
            if (!SyncChanges)
                return;
            string _allPlayersRemoteIdsJson = Serializer.ToClient(_team.Players.GetRemoteIds());
            var playersToSyncTo = _team.Players.GetAllArrayExcept(player);
            NAPI.Task.RunSafe(() =>
            {
                player.TriggerEvent(ToClientEvent.SyncTeamPlayers, _allPlayersRemoteIdsJson);
                NAPI.ClientEvent.TriggerClientEventToPlayers(playersToSyncTo, ToClientEvent.PlayerJoinedTeam, player.RemoteId);

                foreach (var target in playersToSyncTo)
                {
                    if (!player.Relations.HasRelationTo(target, PlayerRelation.Block) && !target.MuteHandler.IsVoiceMuted)
                        target.Voice.SetVoiceTo(player, true);
                    if (!target.Relations.HasRelationTo(player, PlayerRelation.Block) && !player.MuteHandler.IsVoiceMuted)
                        player.Voice.SetVoiceTo(target, true);
                }
            });
        }

        public void SyncAllPlayers()
        {
            if (!SyncChanges)
                return;
            string _allPlayersRemoteIdsJson = Serializer.ToClient(_team.Players.GetRemoteIds());
            var playersToSyncTo = _team.Players.GetAllArray();
            NAPI.Task.RunSafe(() =>
            {
                NAPI.ClientEvent.TriggerClientEventToPlayers(playersToSyncTo, ToClientEvent.SyncTeamPlayers, _allPlayersRemoteIdsJson);
                foreach (var player in playersToSyncTo)
                    foreach (var target in playersToSyncTo)
                    {
                        if (target == player)
                            continue;
                        if (!player.Relations.HasRelationTo(target, PlayerRelation.Block) && !target.MuteHandler.IsVoiceMuted)
                            target.Voice.SetVoiceTo(player, true);
                        if (!target.Relations.HasRelationTo(player, PlayerRelation.Block) && !player.MuteHandler.IsVoiceMuted)
                            player.Voice.SetVoiceTo(target, true);
                    }
            });
        }

        public void SyncRemovedPlayer(ITDSPlayer player)
        {
            if (!SyncChanges)
                return;
            var playersToSyncTo = _team.Players.GetAllArrayExcept(player);
            NAPI.Task.RunSafe(() =>
            {
                player.Voice.ResetVoiceToAndFrom();

                NAPI.ClientEvent.TriggerClientEventToPlayers(playersToSyncTo, ToClientEvent.PlayerLeftTeam, player.RemoteId);
                player.TriggerEvent(ToClientEvent.ClearTeamPlayers);
            });
        }
    }
}
