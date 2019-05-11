using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class PlayerMapFavourites
    {
        public int PlayerId { get; set; }
        public int MapId { get; set; }

        public virtual Maps Map { get; set; }
        public virtual Players Player { get; set; }
    }
}
