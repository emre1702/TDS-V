using System;
using System.Linq;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Players;
using TDS_Server.LobbySystem.TeamHandlers;
using TDS_Shared.Core;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Sync
{
    public class BaseLobbySync
    {
        protected SyncedLobbySettings SyncedSettings { get; private set; }

        private readonly Func<uint> _dimensionProvider;
        protected readonly IBaseLobbyPlayers Players;
        private readonly BaseLobbyTeamsHandler _teams;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public BaseLobbySync(LobbyDb entity, IBaseLobbyEventsHandler events, Func<uint> dimensionProvider, IBaseLobbyPlayers players, BaseLobbyTeamsHandler teams)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            _dimensionProvider = dimensionProvider;
            Players = players;
            _teams = teams;

            InitSyncedSettings(entity);

            events.CreatedAfter += InitSyncedSettings;
            events.PlayerLeftAfter += Events_PlayerLeftLobbyAfter;
            events.PlayerJoined += Events_PlayerJoined;
        }

        protected virtual void InitSyncedSettings(LobbyDb entity)
        {
            SyncedSettings = new SyncedLobbySettings
            (
                Id: entity.Id,
                Name: entity.Name,
                Type: entity.Type,
                IsOfficial: entity.IsOfficial,
                BombDefuseTimeMs: entity.LobbyRoundSettings?.BombDefuseTimeMs,
                BombPlantTimeMs: entity.LobbyRoundSettings?.BombPlantTimeMs,
                SpawnAgainAfterDeathMs: entity.FightSettings?.SpawnAgainAfterDeathMs ?? 400,
                CountdownTime: entity.LobbyRoundSettings?.CountdownTime,
                RoundTime: entity.LobbyRoundSettings?.RoundTime,
                BombDetonateTimeMs: entity.LobbyRoundSettings?.BombDetonateTimeMs,
                InLobbyWithMaps: false,
                MapLimitTime: entity.LobbyMapSettings?.MapLimitTime,
                MapLimitType: entity.LobbyMapSettings?.MapLimitType,
                StartHealth: entity.FightSettings?.StartHealth ?? 100,
                StartArmor: entity.FightSettings?.StartArmor ?? 100,
                IsGangActionLobby: false
            );
        }

        private void Events_PlayerLeftLobbyAfter(ITDSPlayer player)
        {
            NAPI.Task.Run(() =>
            {
                TriggerEvent(ToClientEvent.LeaveSameLobby, player.RemoteId, player.Entity?.Name ?? player.DisplayName);
            });
        }

        private void Events_PlayerJoined(ITDSPlayer player, int teamIndex)
        {
            var playerRemoteIdsJson = Serializer.ToClient(Players.GetPlayers().Select(p => p.RemoteId));
            var syncedTeamDataJson = Serializer.ToClient(_teams.GetTeams().Select(t => t.SyncedTeamData));

            NAPI.Task.Run(() =>
            {
                TriggerEvent(ToClientEvent.JoinSameLobby, player.RemoteId);
                player.TriggerEvent(ToClientEvent.JoinLobby, SyncedSettings.Json, playerRemoteIdsJson, syncedTeamDataJson);
            });
        }

        public void TriggerEvent(string eventName, params object[] args)
        {
            NAPI.ClientEvent.TriggerClientEventInDimension(_dimensionProvider(), eventName, args);
        }
    }
}
