using MessagePack;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Manager.Utility;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Manager.Userpanel
{
    class PlayerStats
    {
        public static async Task<string?> GetData(TDSPlayer player)
        {
            try
            {
                if (player.Entity is null)
                    return null;
                var stats = await GetPlayerStats(player.Entity.Id, true, player);
                return Serializer.ToBrowser(stats);
            }
            catch (Exception ex)
            {
                Logs.ErrorLogsManager.Log("SendPlayerPlayerStats failed: " + ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, player);
                return null;
            }
        }

        public static async Task<PlayerUserpanelStatsDataDto?> GetPlayerStats(int playerId, bool loadLobbyStats = false, TDSPlayer? forPlayer = null)
        {
            using var dbContext = new TDSDbContext();
            dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var data = await dbContext.Players
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
                .Select(p => new PlayerUserpanelStatsDataDto
                {
                    Id = p.Id,
                    AdminLvl = p.AdminLvl,
                    Donation = p.Donation,
                    IsVip = p.IsVip,
                    Name = p.Name,
                    RegisterDateTime = p.RegisterTimestamp,
                    SCName = p.SCName,
                    Gang = p.GangMemberNavigation != null ? p.GangMemberNavigation.Gang.Team.Name : "-",
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
                .FirstOrDefaultAsync();

            if (data == null)
                return null;

            data.Logs = await dbContext.LogAdmins
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
                .ToListAsync();

            foreach (var adminTarget in data.Logs)
            {
                adminTarget.Admin = await dbContext.Players
                    .Where(p => p.Id == adminTarget.SourceId)
                    .Select(p => p.Name)
                    .FirstOrDefaultAsync();
                adminTarget.Lobby = adminTarget.LobbyId.HasValue ? LobbyManager.GetLobby(adminTarget.LobbyId.Value)?.Name : null;
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
                data.RegisterTimestamp = new DateTimeOffset(data.RegisterDateTime).ToString(Constants.DateTimeOffsetFormat);
                data.LastLogin = new DateTimeOffset(data.LastLoginDateTime).ToString(Constants.DateTimeOffsetFormat);

                foreach (var log in data.Logs)
                {
                    log.Timestamp = new DateTimeOffset(log.TimestampDateTime).ToString(Constants.DateTimeOffsetFormat);
                }
            }

            return data;
        }
    }

#nullable disable
    [MessagePackObject]
    public class PlayerUserpanelLobbyStats
    {
        [Key(0)]
        public string Lobby { get; internal set; }

        [Key(1)]
        public int Kills { get; set; }
        [Key(2)]
        public int Assists { get; set; }
        [Key(3)]
        public int Deaths { get; set; }
        [Key(4)]
        public int Damage { get; set; }
        [Key(5)]
        public int TotalKills { get; set; }
        [Key(6)]
        public int TotalAssists { get; set; }
        [Key(7)]
        public int TotalDeaths { get; set; }
        [Key(8)]
        public int TotalDamage { get; set; }
        [Key(9)]
        public int TotalRounds { get; set; }
        [Key(10)]
        public int MostKillsInARound { get; set; }
        [Key(11)]
        public int MostDamageInARound { get; set; }
        [Key(12)]
        public int MostAssistsInARound { get; set; }

        [Key(13)]
        public int TotalMapsBought { get; set; }

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
    }

    [MessagePackObject]
    public class PlayerUserpanelAdminTargetHistoryDataDto
    {
        [Key(0)]
        public string Admin { get; internal set; }
        [Key(1)]
        public string Lobby { get; internal set; }

        [Key(2)]
        public string Type { get; internal set; }
        [Key(3)]
        public bool AsDonator { get; internal set; }
        [Key(4)]
        public bool AsVip { get; internal set; }
        [Key(5)]
        public string Reason { get; internal set; }
        [Key(6)]
        public string Timestamp { get; internal set; }
        [Key(7)]
        public string LengthOrEndTime { get; internal set; }

        [IgnoreMember]
        public int SourceId { get; internal set; }
        [IgnoreMember]
        public int? TargetId { get; internal set; }
        [IgnoreMember]
        public int? LobbyId { get; internal set; }
        [IgnoreMember]
        public DateTime TimestampDateTime { get; internal set; }
    }

    [MessagePackObject]
    public class PlayerUserpanelStatsDataDto
    {
        [Key(0)]
        public int Id { get; internal set; }
        [Key(1)]
        public long TotalMoney { get; internal set; }
        [Key(2)]
        public double MapsRatedAverage { get; internal set; }
        [Key(3)]
        public string RegisterTimestamp { get; internal set; }
        [Key(4)]
        public bool IsVip { get; internal set; }
        [Key(5)]
        public short AdminLvl { get; internal set; }
        [Key(6)]
        public short Donation { get; internal set; }
        [Key(7)]
        public string Name { get; internal set; }
        [Key(8)]
        public string SCName { get; internal set; }
        [Key(9)]
        public string Gang { get; internal set; }
        [Key(10)]
        public int AmountMapsCreated { get; internal set; }
        [Key(11)]
        public string LastLogin { get; internal set; }
        [Key(12)]
        public List<string> BansInLobbies { get; internal set; }
        [Key(13)]
        public double CreatedMapsAverageRating { get; internal set; }
        [Key(14)]
        public int AmountMapsRated { get; internal set; }
        [Key(15)]
        public int? MuteTime { get; internal set; }
        [Key(16)]
        public int Money { get; internal set; }
        [Key(17)]
        public int? VoiceMuteTime { get; internal set; }
        [Key(18)]
        public int PlayTime { get; internal set; }

        [IgnoreMember]
        public DateTime LastLoginDateTime { get; set; }
        [IgnoreMember]
        public DateTime RegisterDateTime { get; set; }

        [Key(19)]
        public List<PlayerUserpanelLobbyStats> LobbyStats { get; internal set; }
        [Key(20)]
        public List<PlayerUserpanelAdminTargetHistoryDataDto> Logs { get; internal set; }
    }
    #nullable restore
}
