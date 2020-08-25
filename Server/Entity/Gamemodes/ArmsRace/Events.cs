﻿using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;

namespace TDS_Server.Entity.Gamemodes.ArmsRace
{
    partial class ArmsRace
    {
        #region Public Methods

        public override void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer)
        {
            if (killer == player)
                return;
            if (CheckRoundEnd(killer))
                return;

            GiveNextWeapon(killer);
        }

        #endregion Public Methods
    }
}