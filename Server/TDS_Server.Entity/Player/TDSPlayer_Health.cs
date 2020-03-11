using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Server.Entity.Player
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
                
                if (ModPlayer is null)
                    return;
                ModPlayer.Armor = value;
            }
        }

        public int Health
        {
            get => _health;
            set
            {
                _health = value;

                if (ModPlayer is null)
                    return;
                ModPlayer.Health = value;
            }
        }
    }
}
