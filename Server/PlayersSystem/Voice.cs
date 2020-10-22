using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.PlayersSystem;

namespace TDS_Server.PlayersSystem
{
    public class Voice : IPlayerVoice
    {
        private readonly List<ITDSPlayer> _settedVoiceTo = new List<ITDSPlayer>();

#nullable disable
        private ITDSPlayer _player;
#nullable enable

        public void Init(ITDSPlayer player)
        {
            _player = player;
        }

        public void ResetVoiceToAndFrom()
        {
            foreach (var target in _settedVoiceTo)
            {
                if (!target.LoggedIn)
                    continue;
                _player.Voice.SetVoiceTo(target, false);
                target.Voice.SetVoiceTo(_player, false);
            }
            _settedVoiceTo.Clear();
        }

        public void SetVoiceTo(ITDSPlayer target, bool on)
        {
            if (on)
            {
                _player.EnableVoiceTo(target);
                _settedVoiceTo.Add(target);
            }
            else
            {
                _player.DisableVoiceTo(target);
                _settedVoiceTo.Remove(target);
            }
        }
    }
}
