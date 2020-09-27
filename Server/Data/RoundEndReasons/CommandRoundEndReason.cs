using System;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas;

namespace TDS_Server.Data.RoundEndReasons
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
