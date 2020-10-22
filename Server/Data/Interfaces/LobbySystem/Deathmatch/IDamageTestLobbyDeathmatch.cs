using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Models;

namespace TDS_Server.Data.Interfaces.LobbySystem.Deathmatch
{
    public interface IDamageTestLobbyDeathmatch : IFightLobbyDeathmatch
    {
        void SetWeaponDamage(DamageTestWeapon weaponDamageData);

        IEnumerable<DamageTestWeapon> GetWeaponDamages();
    }
}
