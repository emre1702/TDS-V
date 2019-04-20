using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class Teams
    {
        public Teams()
        {
            Gangs = new HashSet<Gangs>();
        }

        public uint Id { get; set; }
        public byte Index { get; set; }
        public string Name { get; set; }
        public uint? Lobby { get; set; }
        public byte ColorR { get; set; }
        public byte ColorG { get; set; }
        public byte ColorB { get; set; }
        public byte BlipColor { get; set; }
        public int SkinHash { get; set; }

        public virtual Lobbies LobbyNavigation { get; set; }
        public virtual ICollection<Gangs> Gangs { get; set; }
    }
}
