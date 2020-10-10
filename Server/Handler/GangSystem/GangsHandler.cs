using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Handler.Entities.GangSystem;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.GangSystem
{
    public class GangsHandler
    {
        private readonly DataSyncHandler _dataSyncHandler;
        private readonly TDSDbContext _dbContext;
        private readonly Dictionary<int, IGang> _gangById = new Dictionary<int, IGang>();
        private readonly Dictionary<int, IGang> _gangByPlayerId = new Dictionary<int, IGang>();
        private readonly Dictionary<int, GangMembers> _gangMemberByPlayerId = new Dictionary<int, GangMembers>();

        private readonly IServiceProvider _serviceProvider;

        public GangsHandler(EventsHandler eventsHandler, TDSDbContext dbContext, IServiceProvider serviceProvider,
            DataSyncHandler dataSyncHandler)
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
            _dataSyncHandler = dataSyncHandler;

            eventsHandler.PlayerLoggedIn += SetPlayerIntoHisGang;
            eventsHandler.PlayerLoggedOut += EventsHandler_PlayerLoggedOut;
            eventsHandler.PlayerJoinedGang += EventsHandler_PlayerJoinedGang;
        }

        public IGang None => _gangById[-1];
        public GangRanks NoneRank => None.Entity.Ranks.First();

        public void Add(IGang gang)
        {
            _gangById[gang.Entity.Id] = gang;

            foreach (var member in gang.Entity.Members)
            {
                _gangByPlayerId[member.PlayerId] = gang;
                _gangMemberByPlayerId[member.PlayerId] = member;
            }

            if (gang.Entity.Id < 0)
                gang.Initialized = true;
        }

        public IGang GetById(int id)
        {
            return _gangById[id];
        }

        public IGang? GetByTeamId(int teamId)
        {
            return _gangById.Values.FirstOrDefault(g => g.Entity.TeamId == teamId);
        }

        public void LoadAll()
        {
            _dbContext.Gangs
                .Include(g => g.Members)
                .ThenInclude(m => m.Rank)
                .Include(g => g.Members)
                .ThenInclude(m => m.Player)
                .ThenInclude(p => p.PlayerStats)
                .Include(g => g.RankPermissions)
                .Include(g => g.Ranks)
                .AsNoTracking()
                .ForEach(g =>
                {
                    foreach (var member in g.Members)
                    {
                        member.RankNumber = member.Rank?.Rank;
                        member.Name = member.Player!.Name;
                        member.LastLogin = member.Player.PlayerStats.LastLoginTimestamp;

                        member.Rank = null;
                        member.Player = null;
                    }

                    ActivatorUtilities.CreateInstance<Gang>(_serviceProvider, g);
                });
        }

        private async ValueTask EventsHandler_PlayerJoinedGang((ITDSPlayer player, IGang gang, GangRanks rank) args)
        {
            if (args.player.Entity is null)
                return;

            var gangMember = new GangMembers
            {
                PlayerId = args.player.Entity!.Id,
                RankId = args.rank.Id,
                LastLogin = args.player.Entity.PlayerStats.LastLoginTimestamp
            };

            await args.player.Gang.ExecuteForDBAsync(async dbContext =>
            {
                args.player.Gang.Entity.Members.Add(gangMember);
                await dbContext.SaveChangesAsync();
            });

            _gangByPlayerId.Add(args.player.Entity.Id, args.gang);
            _gangMemberByPlayerId.Add(args.player.Entity.Id, gangMember);

            SetPlayerIntoHisGang(args.player);
        }

        private void SetPlayerIntoHisGang(ITDSPlayer player)
        {
            player.Gang = GetPlayerGang(player);
            player.GangRank = GetPlayerGangRank(player);

            player.Gang.PlayersOnline.Add(player);

            if (player.Entity is { } && player.IsInGang)
            {
                // Update LastLoginTimestamp in gang entity (for gang window)
                player.Gang.Entity.Members.First(m => m.PlayerId == player.Entity.Id).LastLogin = player.Entity.PlayerStats.LastLoginTimestamp;
            }

            _dataSyncHandler.SetData(player, PlayerDataKey.GangId, DataSyncMode.Player, player.Gang.Entity.Id);
        }

        private void EventsHandler_PlayerLoggedOut(ITDSPlayer player)
        {
            player.Gang.PlayersOnline.Remove(player);
        }

        private IGang GetPlayerGang(ITDSPlayer player)
        {
            if (player.Entity != null)
                if (_gangByPlayerId.TryGetValue(player.Entity.Id, out var gang))
                    return gang;

            return None;
        }

        private GangRanks GetPlayerGangRank(ITDSPlayer player)
        {
            if (player.Entity != null)
                if (_gangMemberByPlayerId.TryGetValue(player.Entity.Id, out GangMembers? gangMember))
                    return gangMember.Rank;

            return NoneRank;
        }
    }
}
