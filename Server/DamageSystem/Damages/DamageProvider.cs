using GTANetworkAPI;
using System;
using System.Collections.Generic;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Handler;

namespace TDS_Server.DamageSystem.Damages
{
    public class DamageProvider : IDamageProvider
    {
        private readonly Dictionary<WeaponHash, DamageDto> _damagesDict = new Dictionary<WeaponHash, DamageDto>();

        private readonly WeaponDatasLoadingHandler _weaponDatasLoadingHandler;

        internal DamageProvider(WeaponDatasLoadingHandler weaponDatasLoadingHandler)
        {
            _weaponDatasLoadingHandler = weaponDatasLoadingHandler;
        }

        public void Init(IEnumerable<LobbyWeapons> weapons)
        {
            foreach (var weapon in weapons)
            {
                if (!weapon.Damage.HasValue && !weapon.HeadMultiplicator.HasValue)
                    _damagesDict[weapon.Hash] = _weaponDatasLoadingHandler.GetDefaultDamage(weapon.Hash);
                else
                    _damagesDict[weapon.Hash] = new DamageDto(weapon);
            }
        }

        public int GetDamage(WeaponHash weapon, bool isHeadshot)
        {
            DamageDto? damageModel;
            lock (_damagesDict)
            {
                if (!_damagesDict.TryGetValue(weapon, out damageModel))
                    return 0;
            }
            
            var damage = damageModel.Damage;
            if (isHeadshot)
                damage *= damageModel.HeadMultiplier;
            return (int)Math.Ceiling(damage);
        }

        public void SetDamage(WeaponHash weaponHash, DamageDto damage)
        {
            lock (_damagesDict)
            {
                _damagesDict[weaponHash] = damage;
            }
        }

        public Dictionary<WeaponHash, DamageDto> GetDamages()
        {
            lock (_damagesDict)
            {
                return new Dictionary<WeaponHash, DamageDto>(_damagesDict);
            }
        }
    }
}
