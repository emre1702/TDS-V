using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Entities.GTA.GTAPlayer
{
    partial class TDSPlayer
    {
        private readonly List<ITDSPlayer> _settedVoiceTo = new List<ITDSPlayer>();

        public override void ResetVoiceToAndFrom()
        {
            foreach (var target in _settedVoiceTo)
            {
                if (!target.LoggedIn)
                    continue;
                base.SetVoiceTo(target, false);
                target.SetVoiceTo(this, false);
            }
            _settedVoiceTo.Clear();
        }

        public override void SetVoiceTo(ITDSPlayer target, bool on)
        {
            base.SetVoiceTo(target, on);

            if (on)
                _settedVoiceTo.Add(target);
            else
                _settedVoiceTo.Remove(target);
        }
    }
}
