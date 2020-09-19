using System;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Sync
{
    public class BaseLobbySync
    {
        protected SyncedLobbySettings SyncedSettings { get; private set; }

        private readonly Func<uint> _dimensionProvider;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public BaseLobbySync(LobbyDb entity, IBaseLobbyEventsHandler events, Func<uint> dimensionProvider)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            _dimensionProvider = dimensionProvider;

            InitSyncedSettings(entity);

            events.LobbyCreatedAfter += InitSyncedSettings;
            events.PlayerLeftLobbyAfter += Events_PlayerLeftLobbyAfter;
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
            TriggerEvent(ToClientEvent.LeaveSameLobby, player.RemoteId, player.Entity?.Name ?? player.DisplayName);
        }

        public void TriggerEvent(string eventName, params object[] args)
        {
            NAPI.ClientEvent.TriggerClientEventInDimension(_dimensionProvider(), eventName, args);
        }
    }
}
