using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
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
        public bool InTesting { get; set; }

        public virtual Players Creator { get; set; }
        public virtual ICollection<LobbyMaps> LobbyMaps { get; set; }
        public virtual ICollection<PlayerMapFavourites> PlayerMapFavourites { get; set; }
        public virtual ICollection<PlayerMapRatings> PlayerMapRatings { get; set; }
    }
}
