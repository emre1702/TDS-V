using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerWeaponStats
    {
        void AddWeaponDamage(WeaponHash weaponHash, PedBodyPart pedBodyPart, int damage, bool killed);

        void AddWeaponShot(WeaponHash weaponHash);

        void Init(ITDSPlayer player, IPlayerEvents events);
    }
}
