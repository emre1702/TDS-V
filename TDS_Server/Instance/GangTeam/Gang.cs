using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Instance.Player;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Gang;

namespace TDS_Server.Instance.GangTeam
{
    internal class Gang
    {
        private static readonly Dictionary<int, Gang> _gangById = new Dictionary<int, Gang>();
        public static Gang None => _gangById[0];

        public Gangs Entity;
        public List<TDSPlayer> PlayersOnline = new List<TDSPlayer>();

        public Gang(Gangs entity)
        {
            Entity = entity;
            _gangById[entity.Id] = this;
        }

        public static Gang GetFromId(int id)
        {
            return _gangById[id];
        }

        public static Task LoadAll(TDSNewContext dbContext)
        {
            return dbContext.Gangs.ForEachAsync(g =>
            {
                new Gang(g);
            });
        }
    }
}