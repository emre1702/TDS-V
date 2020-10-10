using System;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Data.RoundEndReasons
{
    public class ErrorRoundEndReason : RoundEndReason
    {
        public ErrorRoundEndReason() : base(null)
        {
        }

        public override Func<ILanguage, string> MessageProvider
            => lang => lang.ERROR_INFO;

        public override bool KillLoserTeam => false;

        public override bool AddToServerStats => false;
    }
}
