using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Entities.Gang;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.GangSystem
{
    public class GangsHandler
    {
        #region Private Fields

        private readonly TDSDbContext _dbContext;
        private readonly Dictionary<int, IGang> _gangById = new Dictionary<int, IGang>();
        private readonly Dictionary<int, IGang> _gangByPlayerId = new Dictionary<int, IGang>();
        private readonly Dictionary<int, GangMembers> _gangMemberByPlayerId = new Dictionary<int, GangMembers>();

        private readonly IEntitiesByInterfaceCreator _entitiesByInterfaceCreator;

        #endregion Private Fields

        #region Public Constructors

        public GangsHandler(EventsHandler eventsHandler, TDSDbContext dbContext, IEntitiesByInterfaceCreator entitiesByInterfaceCreator)
        {
            _dbContext = dbContext;
            _entitiesByInterfaceCreator = entitiesByInterfaceCreator;

            eventsHandler.PlayerLoggedIn += EventsHandler_PlayerLoggedIn;
            eventsHandler.PlayerLoggedOut += EventsHandler_PlayerLoggedOut;
            eventsHandler.PlayerJoinedLobby += EventsHandler_PlayerJoinedLobby;
        }

        #endregion Public Constructors

        #region Public Properties

        public IGang None => _gangById[-1];
        public GangRanks NoneRank => None.Entity.Ranks.First();

        #endregion Public Properties

        #region Public Methods

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

                    _entitiesByInterfaceCreator.Create<IGang>(g);
                });
        }

        #endregion Public Methods

        #region Private Methods

        private void EventsHandler_PlayerJoinedLobby(ITDSPlayer player, ILobby lobby)
        {
            if (!(lobby is IGangLobby gangLobby))
                return;

            if (!player.Gang.Initialized)
                InitGangForFirstTimeToday(player.Gang, gangLobby);
        }

        private void EventsHandler_PlayerLoggedIn(ITDSPlayer player)
        {
            player.Gang = GetPlayerGang(player);
            player.GangRank = GetPlayerGangRank(player);

            player.Gang.PlayersOnline.Add(player);

            if (player.Entity is { } && player.IsInGang)
            {
                // Update LastLoginTimestamp in gang entity (for gang window)
                player.Gang.Entity.Members.First(m => m.PlayerId == player.Entity.Id).LastLogin = player.Entity.PlayerStats.LastLoginTimestamp;
            }

            player.SetClientMetaData(PlayerDataKey.GangId.ToString(), player.Gang.Entity.Id);
        }

        private void EventsHandler_PlayerLoggedOut(ITDSPlayer player)
        {
            player.Gang.PlayersOnline.Remove(player);
        }

        private IGang GetPlayerGang(ITDSPlayer player)
        {
            if (player.Entity != null)
                if (_gangByPlayerId.TryGetValue(player.Entity.Id, out IGang? gang))
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

        private async void InitGangForFirstTimeToday(IGang gang, IGangLobby gangLobby)
        {
            await gangLobby.LoadGangVehicles(gang);
        }

        #endregion Private Methods
    }
}
