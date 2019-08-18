using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Common.Enum.Userpanel;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Userpanel
{
    class PlayerStats
    {
        private const string _dATETIMEOFFSET_FORMAT = "dddd, MMM dd yyyy HH:mm:ss zzz";

        public static async void SendPlayerPlayerStats(TDSPlayer player)
        {
            try
            {
                if (player.Entity == null)
                    return;
                var stats = await GetPlayerStats(player.Entity.Id);
                NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.LoadUserpanelData, (int)EUserpanelLoadDataType.MyStats, JsonConvert.SerializeObject(stats));
            }
            catch (Exception ex)
            {
                Logs.ErrorLogsManager.Log("SendPlayerPlayerStats failed: " + ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, player);
            }
        }

        private static async Task<PlayerUserpanelStatsDataDto> GetPlayerStats(int playerId)
        {
            using var dbContext = new TDSNewContext();
            dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var data = await dbContext.Players
                .Include(p => p.Gang)
                    .ThenInclude(g => g.Team)
                .Include(p => p.Maps)
                    .ThenInclude(m => m.PlayerMapRatings)
                .Include(p => p.PlayerBansPlayer)
                    .ThenInclude(b => b.Lobby)
                .Include(p => p.PlayerMapRatings)
                .Include(p => p.PlayerStats)
                .Include(p => p.PlayerTotalStats)
                .Where(p => p.Id == playerId)
                .Select(p => new PlayerUserpanelStatsDataDto
                {
                    Id = p.Id,
                    AdminLvl = p.AdminLvl,
                    Donation = p.Donation,
                    IsVip = p.IsVip,
                    Name = p.Name,
                    RegisterTimestamp = new DateTimeOffset(p.RegisterTimestamp).ToString(_dATETIMEOFFSET_FORMAT),
                    SCName = p.SCName,
                    Gang = p.Gang.Team.Name,
                    AmountMapsCreated = p.Maps.Count,
                    CreatedMapsAverageRating = p.Maps.Average(map => map.PlayerMapRatings.Average(rating => rating.Rating)),
                    BansInLobbies = p.PlayerBansPlayer.Select(b => b.Lobby.Name),
                    AmountMapsRated = p.PlayerMapRatings.Count,
                    MapsRatedAverage = p.PlayerMapRatings.Average(m => m.Rating),
                    LastLogin = new DateTimeOffset(p.PlayerStats.LastLoginTimestamp).ToString(_dATETIMEOFFSET_FORMAT),
                    Money = p.PlayerStats.Money,
                    MuteTime = p.PlayerStats.MuteTime,
                    PlayTime = p.PlayerStats.PlayTime,
                    VoiceMuteTime = p.PlayerStats.VoiceMuteTime,
                    TotalMoney = p.PlayerTotalStats.Money 
                })
                .FirstOrDefaultAsync();

            data.Logs = await dbContext.LogAdmins
                .Select(l => new PlayerUserpanelAdminTargetHistoryDataDto
                {
                    SourceId = l.Source,
                    TargetId = l.Target,
                    AsDonator = l.AsDonator,
                    AsVip = l.AsVip,
                    LobbyId = l.Lobby,
                    Reason = l.Reason,
                    Timestamp = new DateTimeOffset(l.Timestamp).ToString(_dATETIMEOFFSET_FORMAT),
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

            return data;
        }
    }

    #nullable disable
    class PlayerUserpanelAdminTargetHistoryDataDto
    {
        public string Admin { get; internal set; }
        public string Lobby { get; internal set; }

        public string Type { get; internal set; }
        public bool AsDonator { get; internal set; }
        public bool AsVip { get; internal set; }
        public string Reason { get; internal set; }
        public string Timestamp { get; internal set; }
        public string LengthOrEndTime { get; internal set; }

        [JsonIgnore]
        public int SourceId { get; internal set; }
        [JsonIgnore]
        public int? TargetId { get; internal set; }
        [JsonIgnore]
        public int? LobbyId { get; internal set; }
    }

    class PlayerUserpanelStatsDataDto
    {
        public int Id { get; internal set; }
        public long TotalMoney { get; internal set; }
        public double MapsRatedAverage { get; internal set; }
        public string RegisterTimestamp { get; internal set; }
        public bool IsVip { get; internal set; }
        public short AdminLvl { get; internal set; }
        public short Donation { get; internal set; }
        public string Name { get; internal set; }
        public string SCName { get; internal set; }
        public string Gang { get; internal set; }
        public int AmountMapsCreated { get; internal set; }
        public string LastLogin { get; internal set; }
        public IEnumerable<string> BansInLobbies { get; internal set; }
        public double CreatedMapsAverageRating { get; internal set; }
        public int AmountMapsRated { get; internal set; }
        public int? MuteTime { get; internal set; }
        public int Money { get; internal set; }
        public int? VoiceMuteTime { get; internal set; }
        public int PlayTime { get; internal set; }

        public IEnumerable<PlayerUserpanelAdminTargetHistoryDataDto> Logs { get; internal set; }
    }
    #nullable restore
}
