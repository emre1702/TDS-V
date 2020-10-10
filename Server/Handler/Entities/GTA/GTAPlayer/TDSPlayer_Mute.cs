using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Entities.GTA.GTAPlayer
{
    partial class TDSPlayer
    {
        public override bool IsMuted => Entity?.PlayerStats.MuteTime.HasValue ?? false;

        public override bool IsPermamuted
        {
            get
            {
                if (Entity is null)
                    return false;
                return Entity.PlayerStats.MuteTime.HasValue && Entity.PlayerStats.MuteTime.Value == 0;
            }
        }

        public override bool IsVoiceMuted => Entity?.PlayerStats.VoiceMuteTime.HasValue ?? false;

        public override int? MuteTime
        {
            get => Entity?.PlayerStats.MuteTime;
            set
            {
                if (Entity != null)
                    Entity.PlayerStats.MuteTime = value;
            }
        }

        public override int? VoiceMuteTime
        {
            get => Entity?.PlayerStats.VoiceMuteTime;
            set
            {
                if (Entity != null)
                    Entity.PlayerStats.VoiceMuteTime = value;
            }
        }

        public override void ChangeMuteTime(ITDSPlayer admin, int minutes, string reason)
        {
            if (Entity is null)
                return;
            _chatHandler.OutputMuteInfo(admin.DisplayName, Entity.Name, minutes, reason);
            MuteTime = minutes == -1 ? 0 : (minutes == 0 ? (int?)null : minutes);
        }

        public override void ChangeVoiceMuteTime(ITDSPlayer admin, int minutes, string reason)
        {
            if (Entity is null)
                return;
            _chatHandler.OutputVoiceMuteInfo(admin.DisplayName, Entity.Name, minutes, reason);
            VoiceMuteTime = minutes == -1 ? 0 : (minutes == 0 ? (int?)null : minutes);

            if (VoiceMuteTime is { } && Team is { })
            {
                Team.Players.DoInMain(player =>
                {
                    SetVoiceTo(player, false);
                });
            }
        }
    }
}
