using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class PlayerMapFavourites
    {
        public uint Id { get; set; }
        public int MapId { get; set; }

        public virtual Players IdNavigation { get; set; }
        public virtual Maps Map { get; set; }
    }
}
