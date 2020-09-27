using System;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Data.RoundEndReasons
{
    public class TargetEmptyRoundEndReason : RoundEndReason
    {
        public TargetEmptyRoundEndReason() : base(null)
        {
        }

        public override Func<ILanguage, string> MessageProvider
            => lang => lang.ROUND_END_TARGET_EMPTY_INFO;

        public override bool KillLoserTeam => true;

        public override bool AddToServerStats => true;
    }
}
