using System;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Data.RoundEndReasons
{
    public class NewPlayerRoundEndReason : RoundEndReason
    {
        public NewPlayerRoundEndReason() : base(null)
        {
        }

        public override Func<ILanguage, string> MessageProvider
            => lang => lang.ROUND_END_NEW_PLAYER_INFO;

        public override bool KillLoserTeam => false;

        public override bool AddToServerStats => false;
    }
}
