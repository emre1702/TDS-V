using GTANetworkAPI;
using TDS_Common.Default;
using TDS_Server.Instance.Player;

namespace TDS_Server.Dto
{
    internal class RoundStatsDto
    {
        private TDSPlayer _player;
        private uint _kills;
        private uint _assists;
        private uint _damage;

        public uint Kills
        {
            get => _kills;
            set
            {
                _kills = value;
                NAPI.ClientEvent.TriggerClientEvent(_player.Client, DToClientEvent.SetKillsForRoundStats, value);
            }
        }

        public uint Assists
        {
            get => _assists;
            set
            {
                _assists = value;
                NAPI.ClientEvent.TriggerClientEvent(_player.Client, DToClientEvent.SetAssistsForRoundStats, value);
            }
        }

        public uint Damage
        {
            get => _damage;
            set
            {
                _damage = value;
                NAPI.ClientEvent.TriggerClientEvent(_player.Client, DToClientEvent.SetDamageForRoundStats, value);
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