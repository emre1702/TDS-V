using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Gang;

namespace TDS_Server.Instance.GangTeam
{
    public class Gang : EntityWrapperClass
    {
        private static readonly Dictionary<int, Gang> _gangById = new Dictionary<int, Gang>();
        private static readonly Dictionary<int, Gang> _gangByPlayerId = new Dictionary<int, Gang>();
        private static readonly Dictionary<int, GangMembers> _gangMemberByPlayerId = new Dictionary<int, GangMembers>();

        public static Gang None => _gangById[-1];
        public static GangRanks NoneRank => None.Entity.Ranks.First();

        public Gangs Entity;
        public List<TDSPlayer> PlayersOnline = new List<TDSPlayer>();
        #nullable disable
        // This can't be null! 
        // If it's null, we got serious problems in the code!
        // Every gang needs a team in GangLobby! Even "None" gang (spectator team)!
        public Team GangLobbyTeam { get; set; }
        #nullable restore

        public bool InAction { get; set; }

        public Gang(Gangs entity)
        {
            Entity = entity;
            _gangById[entity.Id] = this;

            foreach (var member in entity.Members)
            {
                _gangByPlayerId[member.PlayerId] = this;
                _gangMemberByPlayerId[member.PlayerId] = member;
            }

            DbContext.Attach(entity);
        }


        public static Gang GetById(int id)
        {
            return _gangById[id];
        }

        public static Gang? GetByTeamId(int teamId)
        {
            return _gangById.Values.FirstOrDefault(g => g.Entity.TeamId == teamId);
        }

        public static Gang GetPlayerGang(TDSPlayer player)
        {
            if (player.Entity != null)
                if (_gangByPlayerId.ContainsKey(player.Entity.Id))
                    return _gangByPlayerId[player.Entity.Id];
            
            return None;
        }

        public static GangRanks GetPlayerGangRank(TDSPlayer player) 
        {
            if (player.Entity != null)
                if (_gangMemberByPlayerId.ContainsKey(player.Entity.Id))
                    return _gangMemberByPlayerId[player.Entity.Id].RankNavigation;

            return NoneRank;
        }

        public static Task LoadAll(TDSDbContext dbContext)
        {
            return dbContext.Gangs
                .Include(g => g.Members)
                .ThenInclude(m => m.RankNavigation)
                .Include(g => g.RankPermissions)
                .AsNoTracking()
                .ForEachAsync(g =>
            {
                new Gang(g);
            });
        }
    }
}