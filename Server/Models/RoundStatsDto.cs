using GTANetworkAPI;
using TDS_Common.Default;
using TDS_Server.Default;
using TDS_Server.Instance.PlayerInstance;

namespace TDS_Server.Dto
{
    public class RoundStatsDto
    {
        private TDSPlayer _player;
        private int _kills;
        private int _assists;
        private int _damage;

        public int Kills
        {
            get => _kills;
            set
            {
                _kills = value;
                NAPI.ClientEvent.TriggerClientEvent(_player.Player, DToClientEvent.ToBrowserEvent, DToBrowserEvent.SetKillsForRoundStats, value);
            }
        }

        public int Assists
        {
            get => _assists;
            set
            {
                _assists = value;
                NAPI.ClientEvent.TriggerClientEvent(_player.Player, DToClientEvent.ToBrowserEvent, DToBrowserEvent.SetAssistsForRoundStats, value);
            }
        }

        public int Damage
        {
            get => _damage;
            set
            {
                _damage = value;
                NAPI.ClientEvent.TriggerClientEvent(_player.Player, DToClientEvent.ToBrowserEvent, DToBrowserEvent.SetDamageForRoundStats, value);
            }
        }

        public RoundStatsDto(TDSPlayer player)
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
