using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas;

namespace TDS.Server.Data.RoundEndReasons
{
    public class CommandRoundEndReason : RoundEndReason
    {
        private readonly ITDSPlayer _player;

        public CommandRoundEndReason(ITDSPlayer player) : base(null)
            => _player = player;

        public override Func<ILanguage, string> MessageProvider
            => lang => string.Format(lang.ROUND_END_COMMAND_INFO, _player.DisplayName);

        public override bool KillLoserTeam => false;

        public override bool AddToServerStats => false;
    }
}
