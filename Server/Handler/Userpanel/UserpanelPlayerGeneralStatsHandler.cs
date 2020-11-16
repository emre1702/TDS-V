using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Models.Userpanel.History;
using TDS.Server.Data.Models.Userpanel.Stats;
using TDS.Server.Database.Entity;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler.Entities;
using TDS.Shared.Core;
using TDS.Shared.Data.Default;

namespace TDS.Server.Handler.Userpanel
{
    public class UserpanelPlayerGeneralStatsHandler : DatabaseEntityWrapper
    {
        private readonly LobbiesHandler _lobbiesHandler;

        public UserpanelPlayerGeneralStatsHandler(TDSDbContext dbContext, LobbiesHandler lobbiesHandler) : base(dbContext)
            => (_lobbiesHandler) = (lobbiesHandler);

        public async Task<string?> GetData(ITDSPlayer player)
        {
            try
            {
                if (player.Entity is null)
                    return null;
                var stats = await GetPlayerGeneralStats(player.Entity.Id, true, player).ConfigureAwait(false);
                return Serializer.ToBrowser(stats);
            }
            catch (Exception ex)
            {
                var baseEx = ex.GetBaseException();
                LoggingHandler.Instance.LogError("SendPlayerPlayerStats failed: " + baseEx.Message, ex.StackTrace ?? Environment.StackTrace,
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
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false))
                .ConfigureAwait(false);

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
                    .ToListAsync()
                    .ConfigureAwait(false))
                .ConfigureAwait(false);

            foreach (var adminTarget in data.Logs)
            {
                adminTarget.Admin = await ExecuteForDBAsync(async dbContext
                    => await dbContext.Players
                        .Where(p => p.Id == adminTarget.SourceId)
                        .Select(p => p.Name)
                        .FirstOrDefaultAsync()
                        .ConfigureAwait(false))
                    .ConfigureAwait(false);
                adminTarget.Lobby = adminTarget.LobbyId.HasValue ? _lobbiesHandler.GetLobby(adminTarget.LobbyId.Value)?.Entity.Name : null;
            }

            if (forPlayer != null)
            {
                data.RegisterTimestamp = forPlayer
                    .Timezone.GetLocalDateTimeString(data.RegisterDateTime);

                data.LastLogin = forPlayer
                    .Timezone.GetLocalDateTimeString(data.LastLoginDateTime);

                foreach (var log in data.Logs)
                {
                    log.Timestamp = forPlayer
                        .Timezone.GetLocalDateTimeString(log.TimestampDateTime);
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
    }
}
