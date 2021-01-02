using System;
using System.Collections.Generic;
using TDS.Server.Database.Entity.GangEntities;
using TDS.Server.Database.Entity.LobbyEntities;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Database.Entity.Rest
{
    public class Maps
    {
        public int Id { get; set; }
        public int? CreatorId { get; set; }
        public string Name { get; set; }
        public DateTime CreateTimestamp { get; set; }

        public virtual Players Creator { get; set; }
        public virtual ICollection<PlayerMapCreatorRewardsWhileOffline> CreatorRewardsWhileOffline { get; set; }
        public virtual GangActionAreas GangwarArea { get; set; }
        public virtual ICollection<LobbyMaps> LobbyMaps { get; set; }
        public virtual ICollection<PlayerMapFavourites> PlayerMapFavourites { get; set; }
        public virtual ICollection<PlayerMapRatings> PlayerMapRatings { get; set; }
    }
}