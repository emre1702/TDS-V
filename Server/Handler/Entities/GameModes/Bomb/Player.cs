﻿using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Entities.GameModes.Bomb
{
    partial class Bomb
    {
        public override void RemovePlayer(ITDSPlayer player)
        {
            base.RemovePlayer(player);
            if (_planter == player)
                _planter = null;
        }

        public override void RemovePlayerFromAlive(ITDSPlayer player)
        {
            base.RemovePlayerFromAlive(player);
            if (_bombAtPlayer == player)
                DropBomb();
        }
    }
}