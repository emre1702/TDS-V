﻿using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;

namespace TDS_Server.Entity.Player
{
    partial class TDSPlayer
    {
        #region Public Properties

        public bool IsMuted => Entity?.PlayerStats.MuteTime.HasValue ?? false;

        public bool IsPermamuted
        {
            get
            {
                if (Entity is null)
                    return false;
                return Entity.PlayerStats.MuteTime.HasValue && Entity.PlayerStats.MuteTime.Value == 0;
            }
        }

        public bool IsVoiceMuted => Entity?.PlayerStats.VoiceMuteTime.HasValue ?? false;

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

        #endregion Public Properties

        #region Public Methods

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

            /*if (VoiceMuteTime is { } && Team is { })
            {
                foreach (var player in Team.Players)
                {
                    SetVoiceTo(player, false);
                }
            }*/
        }

        #endregion Public Methods
    }
}