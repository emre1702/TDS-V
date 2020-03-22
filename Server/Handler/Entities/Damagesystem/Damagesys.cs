﻿using System.Collections.Generic;
using System.Linq;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Core.Damagesystem
{
    public partial class Damagesys
    {
        public bool DamageDealtThisRound => _allHitters.Count > 0;

        private readonly IModAPI _modAPI;
        private readonly ILoggingHandler _loggingHandler;

        public Damagesys(ICollection<LobbyWeapons> weapons, ICollection<LobbyKillingspreeRewards> killingspreeRewards, IModAPI modAPI, ILoggingHandler loggingHandler, 
            WeaponDatasLoadingHandler weaponDatasLoadingHandler)
        {
            _modAPI = modAPI;
            _loggingHandler = loggingHandler;

            foreach (LobbyWeapons weapon in weapons)
            {
                if (!weapon.Damage.HasValue && !weapon.HeadMultiplicator.HasValue)
                    _damagesDict[weapon.Hash] = weaponDatasLoadingHandler.DefaultDamages[weapon.Hash];
                else
                    _damagesDict[weapon.Hash] = new DamageDto(weapon);
            }
            InitKillingSpreeRewards(killingspreeRewards);
        }

        public void Clear()
        {
            _allHitters.Clear();
        }
    }
}
