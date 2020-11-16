using System.Collections.Generic;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Models;

namespace TDS.Server.Data.Interfaces.LobbySystem.Deathmatch
{
    public interface IDamageTestLobbyDeathmatch : IFightLobbyDeathmatch
    {
        void SetWeaponDamage(DamageTestWeapon weaponDamageData);

        IEnumerable<DamageTestWeapon> GetWeaponDamages();
    }
}
