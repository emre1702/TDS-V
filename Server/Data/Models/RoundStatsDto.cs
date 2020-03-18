using TDS_Server.Data.Defaults;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Data.Models
{
    public class RoundStatsDto
    {
        private ITDSPlayer _player;
        private int _kills;
        private int _assists;
        private int _damage;

        public int Kills
        {
            get => _kills;
            set
            {
                _kills = value;
                _player.SendBrowserEvent(ToBrowserEvent.SetKillsForRoundStats, value);
            }
        }

        public int Assists
        {
            get => _assists;
            set
            {
                _assists = value;
                _player.SendBrowserEvent(ToBrowserEvent.SetAssistsForRoundStats, value);
            }
        }

        public int Damage
        {
            get => _damage;
            set
            {
                _damage = value;
                _player.SendBrowserEvent(ToBrowserEvent.SetDamageForRoundStats, value);
            }
        }

        public RoundStatsDto(ITDSPlayer player)
        {
            _player = player;
            Clear();  // to sync it
        }

        public void Clear()
        {
            Kills = 0;
            Assists = 0;
            Damage = 0;
        }
    }
}
