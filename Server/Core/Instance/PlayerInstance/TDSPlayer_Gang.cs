using TDS_Server.Instance.GangTeam;
using TDS_Server_DB.Entity.GangEntities;

namespace TDS_Server.Core.Instance.PlayerInstance
{
    partial class TDSPlayer
    {
        private Gang? _gang;

        public Gang Gang
        {
            get
            {
                if (_gang is null)
                {
                    _gang = Gang.None;
                }
                return _gang;
            }
            set => _gang = value;
        }

        public GangRanks? GangRank { get; set; }
    }
}
