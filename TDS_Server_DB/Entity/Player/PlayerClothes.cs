﻿namespace TDS_Server_DB.Entity.Player
{
    public partial class PlayerClothes
    {
        public int PlayerId { get; set; }
        public bool IsMale { get; set; }

        public virtual Players Player { get; set; }
    }
}
