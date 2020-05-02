using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Handler.Entities.GangSystem;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.GangSystem
{
    public class GangsHandler
    {
        private readonly Dictionary<int, Gang> _gangById = new Dictionary<int, Gang>();
        private readonly Dictionary<int, Gang> _gangByPlayerId = new Dictionary<int, Gang>();
        private readonly Dictionary<int, GangMembers> _gangMemberByPlayerId = new Dictionary<int, GangMembers>();

        public Gang None => _gangById[-1];
        public GangRanks NoneRank => None.Entity.Ranks.First();

        private readonly TDSDbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly DataSyncHandler _dataSyncHandler;

        public GangsHandler(EventsHandler eventsHandler, TDSDbContext dbContext, IServiceProvider serviceProvider,
            DataSyncHandler dataSyncHandler)
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
            _dataSyncHandler = dataSyncHandler;

            eventsHandler.PlayerLoggedIn += EventsHandler_PlayerLoggedIn;
            eventsHandler.PlayerLoggedOut += EventsHandler_PlayerLoggedOut;
            eventsHandler.PlayerJoinedLobby += EventsHandler_PlayerJoinedLobby;
        }

        public void LoadAll()
        {
            _dbContext.Gangs
                .Include(g => g.Members)
                .ThenInclude(m => m.RankNavigation)
                .Include(g => g.RankPermissions)
                .Include(g => g.Ranks)
                .AsNoTracking()
                .ForEach(g =>
                {
                    ActivatorUtilities.CreateInstance<Gang>(_serviceProvider, g);
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

            if (gang.Entity.Id < 0)
                gang.Initialized = true;
        }

        public Gang GetById(int id)
        {
            return _gangById[id];
        }

        public Gang? GetByTeamId(int teamId)
        {
            return _gangById.Values.FirstOrDefault(g => g.Entity.TeamId == teamId);
        }

        private IGang GetPlayerGang(ITDSPlayer player)
        {
            if (player.Entity != null)
                if (_gangByPlayerId.TryGetValue(player.Entity.Id, out Gang? gang))
                    return gang;

            return None;
        }

        private GangRanks GetPlayerGangRank(ITDSPlayer player)
        {
            if (player.Entity != null)
                if (_gangMemberByPlayerId.TryGetValue(player.Entity.Id, out GangMembers? gangMember))
                    return gangMember.RankNavigation;

            return NoneRank;
        }


        private async void InitGangForFirstTimeToday(IGang gang, GangLobby gangLobby)
        {
            await gangLobby.LoadGangVehicles(gang);
        }


        private void EventsHandler_PlayerLoggedIn(ITDSPlayer player)
        {
            player.Gang = GetPlayerGang(player);
            player.GangRank = GetPlayerGangRank(player);

            player.Gang.PlayersOnline.Add(player);

            _dataSyncHandler.SetData(player, PlayerDataKey.GangId, DataSyncMode.Player, player.Gang.Entity.Id);
        }

        private void EventsHandler_PlayerLoggedOut(ITDSPlayer player)
        {
            player.Gang.PlayersOnline.Remove(player);
        }

        private void EventsHandler_PlayerJoinedLobby(ITDSPlayer player, ILobby lobby)
        {
            if (!(lobby is GangLobby gangLobby))
                return;

            if (!player.Gang.Initialized)
                InitGangForFirstTimeToday(player.Gang, gangLobby);
        }
    }
}
