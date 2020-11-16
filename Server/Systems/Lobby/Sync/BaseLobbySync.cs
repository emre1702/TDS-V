using System;
using System.Linq;
using System.Threading.Tasks;
using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.Players;
using TDS.Server.Data.Interfaces.LobbySystem.Sync;
using TDS.Server.Handler.Extensions;
using TDS.Server.LobbySystem.TeamHandlers;
using TDS.Shared.Core;
using TDS.Shared.Data.Models;
using TDS.Shared.Default;
using LobbyDb = TDS.Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS.Server.LobbySystem.Sync
{
    public class BaseLobbySync : IBaseLobbySync
    {
        protected IBaseLobby Lobby { get; }

#nullable disable
        protected SyncedLobbySettings SyncedSettings { get; private set; }
#nullable enable

        protected IBaseLobbyEventsHandler Events { get; }

        public BaseLobbySync(IBaseLobby lobby, IBaseLobbyEventsHandler events)
        {
            Lobby = lobby;
            Events = events;

            events.CreatedAfter += Events_CreatedAfter;
            events.PlayerLeftAfter += Events_PlayerLeftLobbyAfter;
            events.PlayerJoined += Events_PlayerJoined;
            events.RemoveAfter += RemoveEvents;
        }

        protected virtual void RemoveEvents(IBaseLobby lobby)
        {
            Events.CreatedAfter -= Events_CreatedAfter;
            if (Events.PlayerLeftAfter is { })
                Events.PlayerLeftAfter -= Events_PlayerLeftLobbyAfter;
            if (Events.PlayerJoined is { })
                Events.PlayerJoined -= Events_PlayerJoined;
            Events.RemoveAfter -= RemoveEvents;
        }

        protected virtual SyncedLobbySettings CreateSyncedSettings(LobbyDb entity)
        {
            return new SyncedLobbySettings
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

        private void Events_CreatedAfter(LobbyDb entity)
        {
            SyncedSettings = CreateSyncedSettings(entity);
            SyncedSettings.Json = Serializer.ToClient(SyncedSettings);
        }

        private ValueTask Events_PlayerLeftLobbyAfter((ITDSPlayer Player, int HadLifes) data)
        {
            NAPI.Task.RunSafe(() =>
            {
                TriggerEvent(ToClientEvent.LeaveSameLobby, data.Player.RemoteId, data.Player.Entity?.Name ?? data.Player.DisplayName);
            });
            return default;
        }

        private ValueTask Events_PlayerJoined((ITDSPlayer Player, int TeamIndex) data)
        {
            var playerRemoteIdsJson = Serializer.ToClient(Lobby.Players.GetPlayers().Select(p => p.RemoteId));
            var syncedTeamDataJson = Serializer.ToClient(Lobby.Teams.GetTeams().Select(t => t.SyncedData));

            NAPI.Task.RunSafe(() =>
            {
                TriggerEvent(ToClientEvent.JoinSameLobby, data.Player.RemoteId);
                data.Player.TriggerEvent(ToClientEvent.JoinLobby, SyncedSettings.Json, playerRemoteIdsJson, syncedTeamDataJson);
            });
            return default;
        }

        public void TriggerEvent(string eventName, params object[] args)
        {
            NAPI.Task.RunSafe(() =>
                NAPI.ClientEvent.TriggerClientEventInDimension(Lobby.MapHandler.Dimension, eventName, args));
        }
    }
}
