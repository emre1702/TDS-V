namespace TDS_Server.Instance.PlayerInstance
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
    }
}
