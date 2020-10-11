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
                _player.SetVoiceTo(target, false);
                target.SetVoiceTo(_player, false);
            }
            _settedVoiceTo.Clear();
        }

        public void SetVoiceTo(ITDSPlayer target, bool on)
        {
            _player.SetVoiceTo(target, on);

            if (on)
                _settedVoiceTo.Add(target);
            else
                _settedVoiceTo.Remove(target);
        }
    }
}
