using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Server.Core.Instance.PlayerInstance
{
    partial class TDSPlayer
    {
        private int _armor;
        private int _health;

        public int Armor
        {
            get => _armor;
            set
            {
                _armor = value;
                
                if (Player is null)
                    return;
                Player.Armor = value;
            }
        }

        public int Health
        {
            get => _health;
            set
            {
                _health = value;

                if (Player is null)
                    return;
                Player.Health = value;
            }
        }
    }
}
