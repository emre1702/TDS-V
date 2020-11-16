using System;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.TeamsSystem;

namespace TDS.Server.Data.RoundEndReasons
{
#nullable enable

    public class TimeRoundEndReason : RoundEndReason
    {
        public TimeRoundEndReason(ITeam? winnerTeam) : base(winnerTeam)
        {
        }

        public override Func<ILanguage, string> MessageProvider
            => lang => WinnerTeam is { }
            ? string.Format(lang.ROUND_END_TIME_INFO, WinnerTeam.Entity.Name)
            : lang.ROUND_END_TIME_TIE_INFO;

        public override bool KillLoserTeam => true;

        public override bool AddToServerStats => true;
    }
}
