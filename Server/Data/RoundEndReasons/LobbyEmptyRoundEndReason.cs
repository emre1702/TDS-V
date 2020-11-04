using System;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Data.RoundEndReasons
{
    public class LobbyEmptyRoundEndReason : RoundEndReason
    {
        public LobbyEmptyRoundEndReason() : base(null)
        {
        }

        public override Func<ILanguage, string> MessageProvider => lang => lang.ROUND_END_DEATH_ALL_INFO;

        public override bool KillLoserTeam => false;

        public override bool AddToServerStats => false;
    }
}
