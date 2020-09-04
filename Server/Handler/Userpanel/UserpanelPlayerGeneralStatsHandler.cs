using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities;
using TDS_Shared.Core;
using TDS_Shared.Data.Default;

namespace TDS_Server.Handler.Userpanel
{
#nullable disable

    public class PlayerUserpanelAdminTargetHistoryDataDto
    {
        #region Public Properties

        [JsonProperty("0")]
        public string Admin { get; internal set; }

        [JsonProperty("3")]
        public bool AsDonator { get; internal set; }

        [JsonProperty("4")]
        public bool AsVip { get; internal set; }

        [JsonProperty("7")]
        public string LengthOrEndTime { get; internal set; }

        [JsonProperty("1")]
        public string Lobby { get; internal set; }

        [JsonIgnore]
        public int? LobbyId { get; internal set; }

        [JsonProperty("5")]
        public string Reason { get; internal set; }

        [JsonIgnore]
        public int SourceId { get; internal set; }

        [JsonIgnore]
        public int? TargetId { get; internal set; }

        [JsonProperty("6")]
        public string Timestamp { get; internal set; }

        [JsonIgnore]
        public DateTime TimestampDateTime { get; internal set; }

        [JsonProperty("2")]
        public string Type { get; internal set; }

        #endregion Public Properties
    }

    public class PlayerUserpanelGeneralStatsDataDto
    {
        #region Public Properties

        [JsonProperty("4")]
        public short AdminLvl { get; internal set; }

        [JsonProperty("13")]
        public int AmountMapsCreated { get; internal set; }

        [JsonProperty("16")]
        public int AmountMapsRated { get; internal set; }

        [JsonProperty("12")]
        public List<string> BansInLobbies { get; internal set; }

        [JsonProperty("15")]
        public double CreatedMapsAverageRating { get; internal set; }

        [JsonProperty("5")]
        public short Donation { get; internal set; }

        [JsonProperty("3")]
        public string Gang { get; internal set; }

        [JsonProperty("0")]
        public int Id { get; internal set; }

        [JsonProperty("6")]
        public bool IsVip { get; internal set; }

        [JsonProperty("17")]
        public string LastLogin { get; internal set; }

        [JsonIgnore]
        public DateTime LastLoginDateTime { get; set; }

        [JsonProperty("19")]
        public List<PlayerUserpanelLobbyStats> LobbyStats { get; internal set; }

        [JsonProperty("20")]
        public List<PlayerUserpanelAdminTargetHistoryDataDto> Logs { get; internal set; }

        [JsonProperty("14")]
        public double MapsRatedAverage { get; internal set; }

        [JsonProperty("7")]
        public int Money { get; internal set; }

        [JsonProperty("10")]
        public int? MuteTime { get; internal set; }

        [JsonProperty("1")]
        public string Name { get; internal set; }

        [JsonProperty("9")]
        public int PlayTime { get; internal set; }

        [JsonIgnore]
        public DateTime RegisterDateTime { get; set; }

        [JsonProperty("18")]
        public string RegisterTimestamp { get; internal set; }

        [JsonProperty("2")]
        public string SCName { get; internal set; }

        [JsonProperty("8")]
        public long TotalMoney { get; internal set; }

        [JsonProperty("11")]
        public int? VoiceMuteTime { get; internal set; }

        #endregion Public Properties
    }

    public class PlayerUserpanelLobbyStats
    {
        #region Public Constructors

        public PlayerUserpanelLobbyStats(PlayerLobbyStats stats)
        {
            Lobby = stats.Lobby.Name;

            Kills = stats.Kills;
            Assists = stats.Assists;
            Deaths = stats.Deaths;
            Damage = stats.Damage;

            TotalKills = stats.TotalKills;
            TotalAssists = stats.TotalAssists;
            TotalDeaths = stats.TotalDeaths;
            TotalDamage = stats.TotalDamage;

            TotalRounds = stats.TotalRounds;
            MostKillsInARound = stats.MostKillsInARound;
            MostDamageInARound = stats.MostDamageInARound;
            MostAssistsInARound = stats.MostAssistsInARound;
        }

        #endregion Public Constructors

        #region Public Properties

        [JsonProperty("2")]
        public int Assists { get; set; }

        [JsonProperty("4")]
        public int Damage { get; set; }

        [JsonProperty("3")]
        public int Deaths { get; set; }

        [JsonProperty("1")]
        public int Kills { get; set; }

        [JsonProperty("0")]
        public string Lobby { get; internal set; }

        [JsonProperty("12")]
        public int MostAssistsInARound { get; set; }

        [JsonProperty("11")]
        public int MostDamageInARound { get; set; }

        [JsonProperty("10")]
        public int MostKillsInARound { get; set; }

        [JsonProperty("6")]
        public int TotalAssists { get; set; }

        [JsonProperty("8")]
        public int TotalDamage { get; set; }

        [JsonProperty("7")]
        public int TotalDeaths { get; set; }

        [JsonProperty("5")]
        public int TotalKills { get; set; }

        [JsonProperty("13")]
        public int TotalMapsBought { get; set; }

        [JsonProperty("9")]
        public int TotalRounds { get; set; }

        #endregion Public Properties
    }

#nullable restore

    public class UserpanelPlayerGeneralStatsHandler : DatabaseEntityWrapper
    {
        #region Private Fields

        private readonly LobbiesHandler _lobbiesHandler;
        private readonly Serializer _serializer;

        #endregion Private Fields

        #region Public Constructors

        public UserpanelPlayerGeneralStatsHandler(TDSDbContext dbContext, ILoggingHandler loggingHandler, Serializer serializer, LobbiesHandler lobbiesHandler) : base(dbContext, loggingHandler)
            => (_serializer, _lobbiesHandler) = (serializer, lobbiesHandler);

        #endregion Public Constructors

        #region Public Methods

        public async Task<string?> GetData(ITDSPlayer player)
        {
            try
            {
                if (player.Entity is null)
                    return null;
                var stats = await GetPlayerGeneralStats(player.Entity.Id, true, player);
                return _serializer.ToBrowser(stats);
            }
            catch (Exception ex)
            {
                var baseEx = ex.GetBaseException();
                LoggingHandler.LogError("SendPlayerPlayerStats failed: " + baseEx.Message, ex.StackTrace ?? Environment.StackTrace,
                    ex.GetType().Name + "|" + baseEx.GetType().Name, player);
                return null;
            }
        }

        public async Task<PlayerUserpanelGeneralStatsDataDto?> GetPlayerGeneralStats(int playerId, bool loadLobbyStats = false, ITDSPlayer? forPlayer = null)
        {
            var data = await ExecuteForDBAsync(async dbContext
                => await dbContext.Players
                .Include(p => p.GangMemberNavigation)
                    .ThenInclude(g => g.Gang)
                        .ThenInclude(g => g.Team)
                .Include(p => p.Maps)
                    .ThenInclude(m => m.PlayerMapRatings)
                .Include(p => p.PlayerBansPlayer)
                    .ThenInclude(b => b.Lobby)
                .Include(p => p.PlayerMapRatings)
                .Include(p => p.PlayerStats)
                .Include(p => p.PlayerTotalStats)
                .Include(p => p.PlayerLobbyStats)
                    .ThenInclude(s => s.Lobby)
                .Where(p => p.Id == playerId)
                .AsNoTracking()
                .Select(p => new PlayerUserpanelGeneralStatsDataDto
                {
                    Id = p.Id,
                    AdminLvl = p.AdminLvl,
                    Donation = p.Donation,
                    IsVip = p.IsVip,
                    Name = p.Name,
                    RegisterDateTime = p.RegisterTimestamp,
                    SCName = p.SCName,
                    Gang = p.GangMemberNavigation != null ? p.GangMemberNavigation.Gang.Name : "-",
                    AmountMapsCreated = p.Maps.Count,
                    CreatedMapsAverageRating = p.Maps.Average(map => map.PlayerMapRatings.Average(rating => rating.Rating)),
                    BansInLobbies = p.PlayerBansPlayer.Select(b => b.Lobby.Name).ToList(),
                    AmountMapsRated = p.PlayerMapRatings.Count,
                    MapsRatedAverage = p.PlayerMapRatings.Average(m => m.Rating),
                    LastLoginDateTime = p.PlayerStats.LastLoginTimestamp,
                    Money = p.PlayerStats.Money,
                    MuteTime = p.PlayerStats.MuteTime,
                    PlayTime = p.PlayerStats.PlayTime,
                    VoiceMuteTime = p.PlayerStats.VoiceMuteTime,
                    TotalMoney = p.PlayerTotalStats.Money,

                    LobbyStats = loadLobbyStats ? p.PlayerLobbyStats.Select(s => new PlayerUserpanelLobbyStats(s)).ToList() : null
                })
                .FirstOrDefaultAsync());

            if (data == null)
                return null;

            data.Logs = await ExecuteForDBAsync(async dbContext
                => await dbContext.LogAdmins
                .Select(l => new PlayerUserpanelAdminTargetHistoryDataDto
                {
                    SourceId = l.Source,
                    TargetId = l.Target,
                    AsDonator = l.AsDonator,
                    AsVip = l.AsVip,
                    LobbyId = l.Lobby,
                    Reason = l.Reason,
                    TimestampDateTime = l.Timestamp,
                    Type = l.Type.ToString(),
                    LengthOrEndTime = l.LengthOrEndTime
                })
                .Where(l => l.TargetId == playerId)
                .ToListAsync());

            foreach (var adminTarget in data.Logs)
            {
                adminTarget.Admin = await ExecuteForDBAsync(async dbContext
                => await dbContext.Players
                    .Where(p => p.Id == adminTarget.SourceId)
                    .Select(p => p.Name)
                    .FirstOrDefaultAsync());
                adminTarget.Lobby = adminTarget.LobbyId.HasValue ? _lobbiesHandler.GetLobby(adminTarget.LobbyId.Value)?.Name : null;
            }

            if (forPlayer != null)
            {
                data.RegisterTimestamp = forPlayer
                    .GetLocalDateTimeString(data.RegisterDateTime);

                data.LastLogin = forPlayer
                    .GetLocalDateTimeString(data.LastLoginDateTime);

                foreach (var log in data.Logs)
                {
                    log.Timestamp = forPlayer
                        .GetLocalDateTimeString(log.TimestampDateTime);
                }
            }
            else
            {
                data.RegisterTimestamp = new DateTimeOffset(data.RegisterDateTime).ToString(SharedConstants.DateTimeOffsetFormat);
                data.LastLogin = new DateTimeOffset(data.LastLoginDateTime).ToString(SharedConstants.DateTimeOffsetFormat);

                foreach (var log in data.Logs)
                {
                    log.Timestamp = new DateTimeOffset(log.TimestampDateTime).ToString(SharedConstants.DateTimeOffsetFormat);
                }
            }

            return data;
        }

        #endregion Public Methods
    }
}
