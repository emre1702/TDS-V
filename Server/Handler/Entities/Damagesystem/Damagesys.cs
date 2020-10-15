using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Handler;

namespace TDS_Server.Core.Damagesystem
{
    public partial class Damagesys : IDamagesys
    {
        private readonly WeaponDatasLoadingHandler _weaponDatasLoadingHandler;

        public Damagesys(IFightLobby lobby, WeaponDatasLoadingHandler weaponDatasLoadingHandler)
        {
            _weaponDatasLoadingHandler = weaponDatasLoadingHandler;

            foreach (var weapon in lobby.Entity.LobbyWeapons)
            {
                if (!weapon.Damage.HasValue && !weapon.HeadMultiplicator.HasValue)
                    _damagesDict[weapon.Hash] = weaponDatasLoadingHandler.DefaultDamages[weapon.Hash];
                else
                    _damagesDict[weapon.Hash] = new DamageDto(weapon);
            }
            InitKillingSpreeRewards(lobby.Entity.LobbyKillingspreeRewards);
        }

        public Damagesys(WeaponDatasLoadingHandler weaponDatasLoadingHandler)
        {
            _weaponDatasLoadingHandler = weaponDatasLoadingHandler;
        }

        public void Init(IEnumerable<LobbyWeapons> weapons, ICollection<LobbyKillingspreeRewards> killingspreeRewards)
        {
            foreach (var weapon in weapons)
            {
                if (!weapon.Damage.HasValue && !weapon.HeadMultiplicator.HasValue)
                    _damagesDict[weapon.Hash] = _weaponDatasLoadingHandler.DefaultDamages[weapon.Hash];
                else
                    _damagesDict[weapon.Hash] = new DamageDto(weapon);
            }
            InitKillingSpreeRewards(killingspreeRewards);
        }

        public bool DamageDealtThisRound => _allHitters.Count > 0;

        public void Clear()
        {
            _allHitters.Clear();
        }
    }
}
