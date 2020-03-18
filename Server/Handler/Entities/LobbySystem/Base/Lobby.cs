using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Database.Entity.Rest;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Entities.TeamSystem;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    public partial class Lobby : DatabaseEntityWrapper, ILobby
    {
        public Lobbies Entity { get; }

        public int Id => Entity.Id;
        public LobbyType Type => Entity.Type;
        public string Name => Entity.Name;
        public bool IsOfficial => Entity.IsOfficial;
        public string CreatorName => Entity.Owner?.Name ?? "?";
        public string OwnerName => CreatorName;
        public int StartTotalHP => (Entity.FightSettings?.StartArmor ?? 100) + (Entity.FightSettings?.StartHealth ?? 100);

        public uint Dimension { get; }
        protected Position3D SpawnPoint { get; }
        public bool IsGangActionLobby { get; set; }

        protected SyncedLobbySettingsDto SyncedLobbySettings;
        protected readonly Serializer Serializer;
        protected readonly IModAPI ModAPI;
        protected readonly LobbiesHandler LobbiesHandler;
        protected readonly SettingsHandler SettingsHandler;
        protected readonly LangHelper LangHelper;
        protected readonly DataSyncHandler DataSyncHandler;
        protected readonly EventsHandler EventsHandler;

        public Lobby(
            Lobbies entity,
            bool isGangActionLobby,

            TDSDbContext dbContext,
            ILoggingHandler loggingHandler,
            Serializer serializer,
            IModAPI modAPI,
            LobbiesHandler lobbiesHandler,
            SettingsHandler settingsHandler,
            LangHelper langHelper,
            DataSyncHandler dataSyncHandler,
            EventsHandler eventsHandler) : base(dbContext, loggingHandler)
        {
            Serializer = serializer;
            ModAPI = modAPI;
            LobbiesHandler = lobbiesHandler;
            SettingsHandler = settingsHandler;
            LangHelper = langHelper;
            DataSyncHandler = dataSyncHandler;
            EventsHandler = eventsHandler;

            Entity = entity;

            dbContext.Attach(entity);

            Dimension = lobbiesHandler.GetFreeDimension();
            SpawnPoint = new Position3D(
                entity.DefaultSpawnX,
                entity.DefaultSpawnY,
                entity.DefaultSpawnZ
            );

            Teams = new List<ITeam>(entity.Teams.Count);
            foreach (Teams teamEntity in entity.Teams.OrderBy(t => t.Index))
            {
                Team team = new Team(serializer, ModAPI, teamEntity);
                Teams.Add(team);
            }

            SyncedLobbySettings = new SyncedLobbySettingsDto
            (
                Id: entity.Id,
                Name: entity.Name,
                Type: entity.Type,
                IsOfficial: entity.IsOfficial,
                BombDefuseTimeMs: entity.LobbyRoundSettings?.BombDefuseTimeMs,
                BombPlantTimeMs: entity.LobbyRoundSettings?.BombPlantTimeMs,
                SpawnAgainAfterDeathMs: entity.FightSettings?.SpawnAgainAfterDeathMs ?? 400,
                CountdownTime: isGangActionLobby ? 0 : entity.LobbyRoundSettings?.CountdownTime,
                RoundTime: entity.LobbyRoundSettings?.RoundTime,
                BombDetonateTimeMs: entity.LobbyRoundSettings?.BombDetonateTimeMs,
                InLobbyWithMaps: this is Arena,
                MapLimitTime: entity.LobbyMapSettings?.MapLimitTime,
                MapLimitType: entity.LobbyMapSettings?.MapLimitType,
                StartHealth: entity.FightSettings?.StartHealth ?? 100,
                StartArmor: entity.FightSettings?.StartArmor ?? 100,
                serializer: serializer
            );

            LobbiesHandler.AddLobby(this);
        }

        public virtual void Start()
        {
        }

        protected async virtual void Remove()
        {
            LobbiesHandler.RemoveLobby(this);

            foreach (TDSPlayer player in Players.ToArray())
            {
                RemovePlayer(player);
            }

            await ExecuteForDBAsync(async (dbContext) =>
            {
                dbContext.Remove(Entity);
                await dbContext.SaveChangesAsync();
                dbContext.Dispose();
            });


        }

        protected bool IsEmpty()
        {
            return Players.Count == 0;
        }


        /// <summary>
        /// Call this on lobby create.
        /// </summary>
        public Task AddToDB()
        {
            return ExecuteForDBAsync(async (dbContext) =>
            {
                dbContext.Add(Entity);
                await dbContext.SaveChangesAsync();

                await dbContext.Entry(Entity)
                    .Reference(e => e.Owner)
                    .LoadAsync();

                await dbContext.Entry(Entity)
                    .Collection(e => e.LobbyMaps)
                    .Query()
                    .Include(e => e.Map)
                    .LoadAsync();

                // Reload again because Entity could have changed (default values in DB)
                SyncedLobbySettings = new SyncedLobbySettingsDto
                (
                    Id: Entity.Id,
                    Name: Entity.Name,
                    Type: Entity.Type,
                    IsOfficial: Entity.IsOfficial,
                    BombDefuseTimeMs: Entity.LobbyRoundSettings?.BombDefuseTimeMs,
                    BombPlantTimeMs: Entity.LobbyRoundSettings?.BombPlantTimeMs,
                    SpawnAgainAfterDeathMs: Entity.FightSettings?.SpawnAgainAfterDeathMs ?? 400,
                    CountdownTime: IsGangActionLobby ? 0 : Entity.LobbyRoundSettings?.CountdownTime,
                    RoundTime: Entity.LobbyRoundSettings?.RoundTime,
                    BombDetonateTimeMs: Entity.LobbyRoundSettings?.BombDetonateTimeMs,
                    InLobbyWithMaps: this is Arena,
                    MapLimitTime: Entity.LobbyMapSettings?.MapLimitTime,
                    MapLimitType: Entity.LobbyMapSettings?.MapLimitType,
                    StartHealth: Entity.FightSettings?.StartHealth ?? 100,
                    StartArmor: Entity.FightSettings?.StartArmor ?? 100,
                    serializer: Serializer
                );
            });

        }
    }
}
