using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Handler.Entities.GangTeam;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Events;

namespace TDS_Server.Handler
{
    public class GangsHandler
    {
        private readonly Dictionary<int, Gang> _gangById = new Dictionary<int, Gang>();
        private readonly Dictionary<int, Gang> _gangByPlayerId = new Dictionary<int, Gang>();
        private readonly Dictionary<int, GangMembers> _gangMemberByPlayerId = new Dictionary<int, GangMembers>();

        public Gang None => _gangById[-1];
        public GangRanks NoneRank => None.Entity.Ranks.First();


        public GangsHandler(EventsHandler eventsHandler, TDSDbContext dbContext, IServiceProvider serviceProvider)
        {
            eventsHandler.PlayerLoggedIn += EventsHandler_PlayerLoggedIn;

            LoadAll(dbContext, serviceProvider);
        }

        public void LoadAll(TDSDbContext dbContext, IServiceProvider serviceProvider)
        {
            dbContext.Gangs
                .Include(g => g.Members)
                .ThenInclude(m => m.RankNavigation)
                .Include(g => g.RankPermissions)
                .Include(g => g.Ranks)
                .AsNoTracking()
                .ForEach(g =>
                {
                    ActivatorUtilities.CreateInstance<Gang>(serviceProvider, g);
                });
        }

        public void Add(Gang gang)
        {
            _gangById[gang.Entity.Id] = gang;

            foreach (var member in gang.Entity.Members)
            {
                _gangByPlayerId[member.PlayerId] = gang;
                _gangMemberByPlayerId[member.PlayerId] = member;
            }
        }

        public Gang GetById(int id)
        {
            return _gangById[id];
        }

        public Gang? GetByTeamId(int teamId)
        {
            return _gangById.Values.FirstOrDefault(g => g.Entity.TeamId == teamId);
        }

        private void EventsHandler_PlayerLoggedIn(ITDSPlayer player)
        {
            player.Gang = GetPlayerGang(player);
            player.GangRank = GetPlayerGangRank(player);
        }

        private IGang GetPlayerGang(ITDSPlayer player)
        {
            if (player.Entity != null)
                if (_gangByPlayerId.ContainsKey(player.Entity.Id))
                    return _gangByPlayerId[player.Entity.Id];

            return None;
        }

        private GangRanks GetPlayerGangRank(ITDSPlayer player)
        {
            if (player.Entity != null)
                if (_gangMemberByPlayerId.ContainsKey(player.Entity.Id))
                    return _gangMemberByPlayerId[player.Entity.Id].RankNavigation;

            return NoneRank;
        }
    }
}
