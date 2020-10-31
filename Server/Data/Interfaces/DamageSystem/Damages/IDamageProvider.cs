using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.DamageSystem.Damages
{
    public interface IDamageProvider
    {
        int GetDamage(WeaponHash weapon, bool isHeadshot);
        void Init(IEnumerable<LobbyWeapons> weapons);
        void SetDamage(WeaponHash weaponHash, DamageDto damage);
        Dictionary<WeaponHash, DamageDto> GetDamages();
    }
}