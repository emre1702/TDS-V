using System;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {
        public int? MuteTime
        {
            get => Entity?.PlayerStats.MuteTime;
            set
            {
                if (Entity != null)
                    Entity.PlayerStats.MuteTime = value;
            }
        }

        public int? VoiceMuteTime
        {
            get => Entity?.PlayerStats.VoiceMuteTime;
            set
            {
                if (Entity != null)
                    Entity.PlayerStats.VoiceMuteTime = value;
            }
        }

        public bool IsMuted => Entity?.PlayerStats.MuteTime.HasValue ?? false;
        public bool IsVoiceMuted => Entity?.PlayerStats.VoiceMuteTime.HasValue ?? false;

        public bool IsPermamuted
        {
            get
            {
                if (Entity is null)
                    return false;
                return Entity.PlayerStats.MuteTime.HasValue && Entity.PlayerStats.MuteTime.Value == 0;
            }
        }


        public void ChangeMuteTime(ITDSPlayer admin, int minutes, string reason)
        {
            if (Entity is null)
                return;
            _chatHandler.OutputMuteInfo(admin.DisplayName, Entity.Name, minutes, reason);
            MuteTime = minutes == -1 ? 0 : (minutes == 0 ? (int?)null : minutes);
        }

        public void ChangeVoiceMuteTime(ITDSPlayer admin, int minutes, string reason)
        {
            if (Entity is null)
                return;
            _chatHandler.OutputVoiceMuteInfo(admin.DisplayName, Entity.Name, minutes, reason);
            VoiceMuteTime = minutes == -1 ? 0 : (minutes == 0 ? (int?)null : minutes);

            if (VoiceMuteTime is { } && Team is { })
            {
                foreach (var player in Team.Players)
                {
                    SetVoiceTo(player, false);
                }
            }

        }
    }
}
