using System.Collections.Generic;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Database.Entity.Rest
{
    public partial class Teams
    {
        #region Public Constructors

        public Teams()
        {
            Gangs = new HashSet<Gangs>();
        }

        #endregion Public Constructors

        #region Public Properties

        public byte BlipColor { get; set; }
        public byte ColorB { get; set; }
        public byte ColorG { get; set; }
        public byte ColorR { get; set; }
        public virtual ICollection<Gangs> Gangs { get; set; }
        public int Id { get; set; }
        public short Index { get; set; }
        public int Lobby { get; set; }
        public virtual Lobbies LobbyNavigation { get; set; }
        public string Name { get; set; }
        public int SkinHash { get; set; }

        #endregion Public Properties

        #region Public Methods

        public Teams DeepCopy()
        {
            return (Teams)MemberwiseClone();
        }

        #endregion Public Methods
    }
}
