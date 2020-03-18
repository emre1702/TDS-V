using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity.GangEntities;

namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {
        private IGang? _gang;

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
    }
}
