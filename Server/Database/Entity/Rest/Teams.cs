using System.Collections.Generic;
using TDS.Server.Database.Entity.GangEntities;
using TDS.Server.Database.Entity.LobbyEntities;

namespace TDS.Server.Database.Entity.Rest
{
    public partial class Teams
    {
        public Teams() => Gangs = new HashSet<Gangs>();

        public byte BlipColor { get; set; }
        public short ColorB { get; set; }
        public short ColorG { get; set; }
        public short ColorR { get; set; }
        public virtual ICollection<Gangs> Gangs { get; set; }
        public int Id { get; set; }
        public short Index { get; set; }
        public int Lobby { get; set; }
        public virtual Lobbies LobbyNavigation { get; set; }
        public string Name { get; set; }
        public int SkinHash { get; set; }

        public Teams DeepCopy()
        {
            return (Teams)MemberwiseClone();
        }
    }
}
