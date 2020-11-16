using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;

namespace TDS.Server.Data.RoundEndReasons
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
