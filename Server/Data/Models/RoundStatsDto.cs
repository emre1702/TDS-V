using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Defaults;

namespace TDS.Server.Data.Models
{
    public class RoundStatsDto
    {
        private int _assists;
        private int _damage;
        private int _kills;
        private readonly ITDSPlayer _player;

        public RoundStatsDto(ITDSPlayer player)
        {
            _player = player;
            Clear();  // to sync it
        }

        public int Assists
        {
            get => _assists;
            set
            {
                _assists = value;
                _player.TriggerBrowserEvent(ToBrowserEvent.SetAssistsForRoundStats, value);
            }
        }

        public int Damage
        {
            get => _damage;
            set
            {
                _damage = value;
                _player.TriggerBrowserEvent(ToBrowserEvent.SetDamageForRoundStats, value);
            }
        }

        public int Kills
        {
            get => _kills;
            set
            {
                _kills = value;
                _player.TriggerBrowserEvent(ToBrowserEvent.SetKillsForRoundStats, value);
            }
        }

        public void Clear()
        {
            Kills = 0;
            Assists = 0;
            Damage = 0;
        }
    }
}
