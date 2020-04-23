﻿using System.Collections.Generic;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {
        private readonly List<ITDSPlayer> _settedVoiceTo = new List<ITDSPlayer>();

        public void SetVoiceTo(ITDSPlayer target, bool on)
        {
            ModPlayer?.SetVoiceTo(target, on);

            if (on)
                _settedVoiceTo.Add(target);
            else 
                _settedVoiceTo.Remove(target);
        }

        public void ResetVoiceToAndFrom()
        {
            foreach (var target in _settedVoiceTo)
            {
                if (!target.LoggedIn)
                    continue;
                ModPlayer?.SetVoiceTo(target, false);
                target.SetVoiceTo(this, false);
            }
        }
    }
}
