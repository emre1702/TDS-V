using TDS_Common.Default;
using TDS_Server.Instance.Player;

namespace TDS_Server.Instance.Lobby
{
    partial class FightLobby
    {
        public void SyncPlayerWeaponUpgradesToAll(TDSPlayer player, uint weaponHash) 
        {
            if (player.WeaponUpgradesDatasJson.TryGetValue(weaponHash, out string? dataJson))
                SendAllOtherPlayerEvent(DToClientEvent.SyncPlayerWeaponUpgrades, player, null, player.Client.Handle.Value, dataJson);
        }
    }
}
