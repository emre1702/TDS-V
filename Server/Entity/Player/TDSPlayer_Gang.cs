using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity.GangEntities;

namespace TDS_Server.Entity.Player
{
    partial class TDSPlayer
    {
        #region Private Fields

        private IGang? _gang;

        #endregion Private Fields

        #region Public Properties

        public IGang Gang
        {
            get
            {
                if (_gang is null)
                {
                    _gang = _gangsHandler.None;
                }
                return _gang;
            }
            set => _gang = value;
        }

        public GangRanks? GangRank { get; set; }
        public bool IsGangOwner => Gang.Entity.OwnerId == Entity?.Id;
        public bool IsInGang => Gang.Entity.Id > 0;

        #endregion Public Properties
    }
}
