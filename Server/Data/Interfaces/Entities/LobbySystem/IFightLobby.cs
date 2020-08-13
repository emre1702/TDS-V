using AltV.Net.Data;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Interfaces.Entities.LobbySystem
{
    public interface IFightLobby : ILobby
    {
        void OnPlayerWeaponSwitch(ITDSPlayer player, WeaponHash previousWeapon, WeaponHash newWeapon);
        void DamagedPlayer(ITDSPlayer target, ITDSPlayer player, WeaponHash weaponHash, BodyPart bodyPart);
        void SpectateNext(ITDSPlayer player, bool forward);
    }
}
