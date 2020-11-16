using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.PlayersSystem;
using TDS.Server.Handler;

namespace TDS.Server.PlayersSystem
{
    public class MuteHandler : IPlayerMuteHandler
    {
        public bool IsMuted => _player.Entity?.PlayerStats.MuteTime.HasValue ?? false;  // ?? false because Entity null should return true
        public bool IsVoiceMuted => _player.Entity?.PlayerStats.VoiceMuteTime.HasValue ?? false;  // ?? false because Entity null should return true

        private readonly ChatHandler _chatHandler;

#nullable disable
        private ITDSPlayer _player;
#nullable enable

        public MuteHandler(ChatHandler chatHandler)
        {
            _chatHandler = chatHandler;
        }

        public void Init(ITDSPlayer player)
        {
            _player = player;
        }

        public bool IsPermamuted
        {
            get
            {
                if (_player.Entity is null)
                    return false;
                return _player.Entity.PlayerStats.MuteTime == 0;
            }
        }

        public int? MuteTime
        {
            get => _player.Entity?.PlayerStats.MuteTime;
            set
            {
                if (_player.Entity != null)
                    _player.Entity.PlayerStats.MuteTime = value;
            }
        }

        public int? VoiceMuteTime
        {
            get => _player.Entity?.PlayerStats.VoiceMuteTime;
            set
            {
                if (_player.Entity != null)
                    _player.Entity.PlayerStats.VoiceMuteTime = value;
            }
        }

        public void ChangeMuteTime(ITDSPlayer admin, int minutes, string reason)
        {
            if (_player.Entity is null)
                return;
            _chatHandler.OutputMuteInfo(admin.DisplayName, _player.Entity.Name, minutes, reason);
            MuteTime = minutes == -1 ? 0 : (minutes == 0 ? (int?)null : minutes);
        }

        public void ChangeVoiceMuteTime(ITDSPlayer admin, int minutes, string reason)
        {
            if (_player.Entity is null)
                return;
            _chatHandler.OutputVoiceMuteInfo(admin.DisplayName, _player.Entity.Name, minutes, reason);
            VoiceMuteTime = minutes == -1 ? 0 : (minutes == 0 ? (int?)null : minutes);

            if (VoiceMuteTime is { } && _player.Team is { })
            {
                _player.Team.Players.DoInMain(player =>
                {
                    _player.Voice.SetVoiceTo(player, false);
                });
            }
        }
    }
}
