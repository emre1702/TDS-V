using System;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Data.RoundEndReasons
{
    public class PlayerWonRoundEndReason : RoundEndReason
    {
        private readonly ITDSPlayer _player;

        public PlayerWonRoundEndReason(ITDSPlayer player) : base(null)
            => _player = player;

        public override Func<ILanguage, string> MessageProvider
            => lang => string.Format(lang.PLAYER_WON_INFO, _player.DisplayName);

        public override bool KillLoserTeam => false;

        public override bool AddToServerStats => true;
    }
}
