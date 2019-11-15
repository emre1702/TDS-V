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
    internal class Gang : EntityWrapperClass
    {
        private static readonly Dictionary<int, Gang> _gangById = new Dictionary<int, Gang>();
        private static readonly Dictionary<int, Gang> _gangByPlayerId = new Dictionary<int, Gang>();
        private static readonly Dictionary<int, GangMembers> _gangMemberByPlayerId = new Dictionary<int, GangMembers>();

        public static Gang None => _gangById[-1];
        public static GangRanks NoneRank => None.Entity.Ranks.First();

        public Gangs Entity;
        public List<TDSPlayer> PlayersOnline = new List<TDSPlayer>();

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

        public static Task LoadAll(TDSNewContext dbContext)
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