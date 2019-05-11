using System.Collections.Generic;
using TDS_Server.Instance.Player;
using TDS_Server_DB.Entity;

namespace TDS_Server.Instance.GangTeam
{
    internal class Gang
    {
        private static readonly Dictionary<int, Gang> gangById = new Dictionary<int, Gang>();
        public static Gang None => gangById[0];

        public Gangs Entity;
        public List<TDSPlayer> PlayersOnline = new List<TDSPlayer>();

        public Gang(Gangs entity)
        {
            Entity = entity;
            gangById[entity.Id] = this;
        }

        public static Gang GetFromId(int id)
        {
            return gangById[id];
        }
    }
}