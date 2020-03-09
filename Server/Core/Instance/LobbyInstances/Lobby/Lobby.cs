using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Dto;
using TDS_Common.Enum;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Instance.Utility;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.LobbyEntities;
using TDS_Server_DB.Entity.Rest;

namespace TDS_Server.Core.Instance.LobbyInstances.Lobby
{
    public partial class Lobby : EntityWrapperClass
    {
        public static readonly Dictionary<int, Lobby> LobbiesByIndex = new Dictionary<int, Lobby>();
        private static readonly HashSet<uint> _dimensionsUsed = new HashSet<uint> { 0 };

        private readonly TDSDbContext _dbContext;
        public Lobbies LobbyEntity { get; }

        public int Id => LobbyEntity.Id;
        public ELobbyType Type => LobbyEntity.Type;
        public string Name => LobbyEntity.Name;
        public bool IsOfficial => LobbyEntity.IsOfficial;
        public string CreatorName => LobbyEntity.Owner?.Name ?? "?";
        public string OwnerName => CreatorName;
        public int StartTotalHP => (LobbyEntity.FightSettings?.StartArmor ?? 100) + (LobbyEntity.FightSettings?.StartHealth ?? 100);

        public uint Dimension { get; }
        protected Vector3 SpawnPoint { get; }
        public bool IsGangActionLobby { get; set; }

        private SyncedLobbySettingsDto _syncedLobbySettings;

        public Lobby(Lobbies entity, bool isGangActionLobby = false)
        {
            _dbContext = new TDSDbContext();
            LobbyEntity = entity;

            _dbContext.Attach(entity);

            Dimension = GetFreeDimension();
            SpawnPoint = new Vector3(
                entity.DefaultSpawnX,
                entity.DefaultSpawnY,
                entity.DefaultSpawnZ
            );

            _dimensionsUsed.Add(Dimension);

            Teams = new List<Team>(entity.Teams.Count);
            foreach (Teams teamEntity in entity.Teams.OrderBy(t => t.Index))
            {
                Team team = new Team(teamEntity);
                Teams.Add(team);
            }

            _syncedLobbySettings = new SyncedLobbySettingsDto
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
                StartArmor: entity.FightSettings?.StartArmor ?? 100
            );
        }

        public virtual void Start()
        {
        }

        protected async virtual void Remove()
        {
            LobbiesByIndex.Remove(LobbyEntity.Id);
            LobbyManager.RemoveLobby(this);
            _dimensionsUsed.Remove(Dimension);

            foreach (TDSPlayer character in Players.ToArray())
            {
                RemovePlayer(character);
            }

            await ExecuteForDBAsync(async (dbContext) =>
            {
                dbContext.Remove(LobbyEntity);
                await dbContext.SaveChangesAsync();
                dbContext.Dispose();
            });


        }

        private static uint GetFreeDimension()
        {
            uint tryid = 0;
            while (_dimensionsUsed.Contains(tryid))
                ++tryid;
            return tryid;
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
                dbContext.Add(LobbyEntity);
                await dbContext.SaveChangesAsync();

                await dbContext.Entry(LobbyEntity)
                    .Reference(e => e.Owner)
                    .LoadAsync();

                await dbContext.Entry(LobbyEntity)
                    .Collection(e => e.LobbyMaps)
                    .Query()
                    .Include(e => e.Map)
                    .LoadAsync();

                // Reload again because LobbyEntity could have changed (default values in DB)
                _syncedLobbySettings = new SyncedLobbySettingsDto
                (
                    Id: LobbyEntity.Id,
                    Name: LobbyEntity.Name,
                    Type: LobbyEntity.Type,
                    IsOfficial: LobbyEntity.IsOfficial,
                    BombDefuseTimeMs: LobbyEntity.LobbyRoundSettings?.BombDefuseTimeMs,
                    BombPlantTimeMs: LobbyEntity.LobbyRoundSettings?.BombPlantTimeMs,
                    SpawnAgainAfterDeathMs: LobbyEntity.FightSettings?.SpawnAgainAfterDeathMs ?? 400,
                    CountdownTime: IsGangActionLobby ? 0 : LobbyEntity.LobbyRoundSettings?.CountdownTime,
                    RoundTime: LobbyEntity.LobbyRoundSettings?.RoundTime,
                    BombDetonateTimeMs: LobbyEntity.LobbyRoundSettings?.BombDetonateTimeMs,
                    InLobbyWithMaps: this is Arena,
                    MapLimitTime: LobbyEntity.LobbyMapSettings?.MapLimitTime,
                    MapLimitType: LobbyEntity.LobbyMapSettings?.MapLimitType,
                    StartHealth: LobbyEntity.FightSettings?.StartHealth ?? 100,
                    StartArmor: LobbyEntity.FightSettings?.StartArmor ?? 100
                );
            });

        }
    }
}
