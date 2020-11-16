using System;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.TeamsSystem;

namespace TDS.Server.Data.RoundEndReasons
{
    public class BombDefusedRoundEndReason : RoundEndReason
    {
        public BombDefusedRoundEndReason(ITeam winnerTeam) : base(winnerTeam)
        {
        }

        public override Func<ILanguage, string> MessageProvider
            => lang => string.Format(lang.ROUND_END_BOMB_DEFUSED_INFO, WinnerTeam.Entity.Name);

        public override bool KillLoserTeam => true;

        public override bool AddToServerStats => true;
    }
}
