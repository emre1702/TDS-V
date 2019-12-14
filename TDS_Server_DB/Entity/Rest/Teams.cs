﻿using System.Collections.Generic;
using TDS_Server_DB.Entity.GangEntities;
using TDS_Server_DB.Entity.LobbyEntities;

namespace TDS_Server_DB.Entity.Rest
{
    public partial class Teams
    {
        public Teams()
        {
            Gangs = new HashSet<Gangs>();
        }

        public int Id { get; set; }
        public short Index { get; set; }
        public string Name { get; set; }
        public int Lobby { get; set; }
        public short ColorR { get; set; }
        public short ColorG { get; set; }
        public short ColorB { get; set; }
        public short BlipColor { get; set; }
        public int SkinHash { get; set; }

        public virtual Lobbies LobbyNavigation { get; set; }
        public virtual ICollection<Gangs> Gangs { get; set; }


        public Teams DeepCopy()
        {
            return (Teams)MemberwiseClone();
        }

    }
}
