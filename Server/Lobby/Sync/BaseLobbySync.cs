using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Shared.Data.Models;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Sync
{
    public class BaseLobbySync
    {
        protected SyncedLobbySettings SyncedSettings { get; private set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public BaseLobbySync(LobbyDb entity, BaseLobbyEventsHandler eventsHandler)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            InitSyncedSettings(entity);

            eventsHandler.LobbyCreatedAfter += InitSyncedSettings;
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
    }
}
