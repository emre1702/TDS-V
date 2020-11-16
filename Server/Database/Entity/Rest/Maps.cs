using System;
using System.Collections.Generic;
using TDS.Server.Database.Entity.GangEntities;
using TDS.Server.Database.Entity.LobbyEntities;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Database.Entity.Rest
{
    public partial class Maps
    {
        #region Public Constructors

        public Maps()
        {
            LobbyMaps = new HashSet<LobbyMaps>();
            PlayerMapFavourites = new HashSet<PlayerMapFavourites>();
            PlayerMapRatings = new HashSet<PlayerMapRatings>();
        }

        #endregion Public Constructors

        #region Public Properties

        public DateTime CreateTimestamp { get; set; }
        public virtual Players Creator { get; set; }
        public int? CreatorId { get; set; }
        public virtual GangwarAreas GangwarArea { get; set; }
        public int Id { get; set; }
        public virtual ICollection<LobbyMaps> LobbyMaps { get; set; }
        public string Name { get; set; }
        public virtual ICollection<PlayerMapFavourites> PlayerMapFavourites { get; set; }
        public virtual ICollection<PlayerMapRatings> PlayerMapRatings { get; set; }

        #endregion Public Properties
    }
}
