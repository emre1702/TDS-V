using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;

namespace TDS_Server.Entity.Gamemodes.Bomb
{
    partial class Bomb
    {
        #region Public Methods

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

        #endregion Public Methods
    }
}
