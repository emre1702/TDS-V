using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity.GangEntities;

namespace TDS_Server.Handler.Entities.GTA.GTAPlayer
{
    partial class TDSPlayer
    {
        private IGang? _gang;

        public override IGang Gang
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

        public override GangRanks? GangRank { get; set; }
        public override bool IsGangOwner => Gang.Entity.OwnerId == Entity?.Id;
        public override bool IsInGang => Gang.Entity.Id > 0;
    }
}
