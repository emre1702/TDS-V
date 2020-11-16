using System;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.TeamsSystem;

namespace TDS.Server.Data.RoundEndReasons
{
#nullable enable

    public class DeathRoundEndReason : RoundEndReason
    {
        public DeathRoundEndReason(ITeam? winnerTeam) : base(winnerTeam)
        {
        }

        public override Func<ILanguage, string> MessageProvider =>
            lang => WinnerTeam is { }
                ? string.Format(lang.ROUND_END_DEATH_INFO, WinnerTeam.Entity.Name)
                : lang.ROUND_END_DEATH_ALL_INFO;

        public override bool KillLoserTeam => false;

        public override bool AddToServerStats => true;
    }
}
