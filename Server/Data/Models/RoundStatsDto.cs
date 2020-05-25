using TDS_Server.Data.Defaults;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Data.Models
{
    public class RoundStatsDto
    {
        #region Private Fields

        private int _assists;
        private int _damage;
        private int _kills;
        private ITDSPlayer _player;

        #endregion Private Fields

        #region Public Constructors

        public RoundStatsDto(ITDSPlayer player)
        {
            _player = player;
            Clear();  // to sync it
        }

        #endregion Public Constructors

        #region Public Properties

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

        public int Kills
        {
            get => _kills;
            set
            {
                _kills = value;
                _player.SendBrowserEvent(ToBrowserEvent.SetKillsForRoundStats, value);
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Clear()
        {
            Kills = 0;
            Assists = 0;
            Damage = 0;
        }

        #endregion Public Methods
    }
}
