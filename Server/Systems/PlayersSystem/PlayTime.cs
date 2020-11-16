using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.PlayersSystem;
using TDS.Shared.Data.Enums.Challenge;

namespace TDS.Server.PlayersSystem
{
    public class PlayTime : IPlayerPlayTime
    {
        public int Minutes
        {
            get => _player.Entity?.PlayerStats.PlayTime ?? 0;
            set
            {
                if (_player.Entity is null)
                    return;
                int addToPlayTime = value - _player.Entity.PlayerStats.PlayTime;
                _player.Entity.PlayerStats.PlayTime = value;
                if (addToPlayTime > 0)
                    _player.Challenges.AddToChallenge(ChallengeType.PlayTime, addToPlayTime);
            }
        }

        public int Hours => (int)Math.Floor(Minutes / 60f);

#nullable disable
        private ITDSPlayer _player;
#nullable enable

        public void Init(ITDSPlayer player)
        {
            _player = player;
        }
    }
}
