using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Entity;
using TDS_Server.Instance.Player;

namespace TDS_Server.Instance.GangTeam
{
    class Gang
    {
        private static readonly Dictionary<uint, Gang> gangById = new Dictionary<uint, Gang>();
        public static Gang None => gangById[0];

        public Gangs Entity;
        public List<TDSPlayer> PlayersOnline = new List<TDSPlayer>();
        

        public Gang(Gangs entity)
        {
            Entity = entity;
            gangById[entity.Id] = this;
        }

        public static Gang GetFromId(uint id)
        {
            return gangById[id];
        }
    }
}
