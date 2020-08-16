using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities.Gang;

namespace TDS_Server.Entity.GangSystem.GangGamemodes.GangwarSystem
{
    partial class GangGangwar
    {
        #region Public Properties

        public IGang? AttackerGang => _gangwarArea.Attacker;

        public IGang? OwnerGang => _gangwarArea.Owner;

        #endregion Public Properties
    }
}
