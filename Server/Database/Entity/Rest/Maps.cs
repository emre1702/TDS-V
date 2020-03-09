using System;
using System.Collections.Generic;
using TDS_Server_DB.Entity.GangEntities;
using TDS_Server_DB.Entity.LobbyEntities;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Database.Entity.Rest
{
    public partial class Maps
    {
        public Maps()
        {
            LobbyMaps = new HashSet<LobbyMaps>();
            PlayerMapFavourites = new HashSet<PlayerMapFavourites>();
            PlayerMapRatings = new HashSet<PlayerMapRatings>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? CreatorId { get; set; }
        public DateTime CreateTimestamp { get; set; }

        public virtual Players Creator { get; set; }
        public virtual ICollection<LobbyMaps> LobbyMaps { get; set; }
        public virtual GangwarAreas GangwarArea { get; set; }
        public virtual ICollection<PlayerMapFavourites> PlayerMapFavourites { get; set; }
        public virtual ICollection<PlayerMapRatings> PlayerMapRatings { get; set; }
    }
}
