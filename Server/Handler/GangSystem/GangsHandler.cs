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
        #region Private Fields

        private readonly DataSyncHandler _dataSyncHandler;
        private readonly TDSDbContext _dbContext;
        private readonly Dictionary<int, Gang> _gangById = new Dictionary<int, Gang>();
        private readonly Dictionary<int, Gang> _gangByPlayerId = new Dictionary<int, Gang>();
        private readonly Dictionary<int, GangMembers> _gangMemberByPlayerId = new Dictionary<int, GangMembers>();

        private readonly IServiceProvider _serviceProvider;

        #endregion Private Fields

        #region Public Constructors

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

        #endregion Public Constructors

        #region Public Properties

        public Gang None => _gangById[-1];
        public GangRanks NoneRank => None.Entity.Ranks.First();

        #endregion Public Properties

        #region Public Methods

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
                    ActivatorUtilities.CreateInstance<Gang>(_serviceProvider, g);
                });
        }

        #endregion Public Methods

        #region Private Methods

        private void EventsHandler_PlayerJoinedLobby(ITDSPlayer player, ILobby lobby)
        {
            if (!(lobby is GangLobby gangLobby))
                return;

            if (!player.Gang.Initialized)
                InitGangForFirstTimeToday(player.Gang, gangLobby);
        }

        private void EventsHandler_PlayerLoggedIn(ITDSPlayer player)
        {
            player.Gang = GetPlayerGang(player);
            player.GangRank = GetPlayerGangRank(player);

            player.Gang.PlayersOnline.Add(player);

            if (player.Entity is { } && player.Gang.Entity.Id > 0)
            {
                // Update LastLoginTimestamp in gang entity (for gang window)
                player.Gang.Entity.Members.First(m => m.PlayerId == player.Entity.Id).Player.PlayerStats.LastLoginTimestamp = player.Entity.PlayerStats.LastLoginTimestamp;
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
                if (_gangByPlayerId.TryGetValue(player.Entity.Id, out Gang? gang))
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

        private async void InitGangForFirstTimeToday(IGang gang, GangLobby gangLobby)
        {
            await gangLobby.LoadGangVehicles(gang);
        }

        #endregion Private Methods
    }
}
