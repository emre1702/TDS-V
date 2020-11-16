using GTANetworkAPI;
using System.Collections.Generic;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.PlayersSystem;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.PlayersSystem
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
            NAPI.Task.RunSafe(() =>
            {
                foreach (var target in _settedVoiceTo)
                {
                    if (!target.LoggedIn)
                        continue;
                    _player.DisableVoiceTo(target);
                    target.DisableVoiceTo(_player);
                }
                _settedVoiceTo.Clear();
            });

        }

        public void SetVoiceTo(ITDSPlayer target, bool toggleOn)
        {
            NAPI.Task.RunSafe(() =>
            {
                if (toggleOn)
                {
                    _player.EnableVoiceTo(target);
                    _settedVoiceTo.Add(target);
                }
                else
                {
                    _player.DisableVoiceTo(target);
                    _settedVoiceTo.Remove(target);
                }
            });
        }
    }
}
