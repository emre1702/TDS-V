using System;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.PlayersSystem;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.PlayersSystem
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
