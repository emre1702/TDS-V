using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Instance.PlayerInstance;

namespace TDS_Server.Instance.GameModes
{
    partial class Bomb
    {
        public override void RemovePlayer(TDSPlayer player)
        {
            base.RemovePlayer(player);
            if (_planter == player)
                _planter = null;
        }

        public override void RemovePlayerFromAlive(TDSPlayer player)
        {
            base.RemovePlayerFromAlive(player);
            if (_bombAtPlayer == player)
                DropBomb();
        }
    }
}
