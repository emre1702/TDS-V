using System;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.TeamsSystem;

namespace TDS.Server.Data.RoundEndReasons
{
    public class TargetEmptyRoundEndReason : RoundEndReason
    {
        public TargetEmptyRoundEndReason(ITeam ownerTeam) : base(ownerTeam)
        {
        }

        public override Func<ILanguage, string> MessageProvider
            => lang => lang.ROUND_END_TARGET_EMPTY_INFO;

        public override bool KillLoserTeam => true;

        public override bool AddToServerStats => true;
    }
}
