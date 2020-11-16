using GTANetworkAPI;
using System.Collections.Generic;
using TDS.Server.Data.Models;
using TDS.Server.Database.Entity.LobbyEntities;

namespace TDS.Server.DamageSystem.Damages
{
    public interface IDamageProvider
    {
        int GetDamage(WeaponHash weapon, bool isHeadshot);
        void Init(IEnumerable<LobbyWeapons> weapons);
        void SetDamage(WeaponHash weaponHash, DamageDto damage);
        Dictionary<WeaponHash, DamageDto> GetDamages();
    }
}