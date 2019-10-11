using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDS_Common.Dto.Sync;
using TDS_Common.Enum;
using TDS_Server.Instance.Lobby;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Logs;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Manager.Utility
{
    class WeaponUpgrades
    {
        public static async void BuyComponent(TDSPlayer player, EWeaponHash weaponHash, EWeaponComponent weaponComponentHash)
        {
            if (player.Entity is null)
                return;

            try
            {
                //todo: Remove money from player (?)
                using var dbContext = new TDSNewContext();
                var component = await dbContext.WeaponComponents.FindAsync(weaponComponentHash, weaponHash);
                if (component == null)
                {
                    ErrorLogsManager.Log($"{player.Client.Name} with ID {player.Entity?.Id ?? '?'} tried to buy weapon-component {weaponComponentHash.ToString()}" 
                                            + $" for weapon {weaponHash.ToString()}, but the component doesn't exist in DB.", Environment.StackTrace, player);
                    NAPI.Notification.SendNotificationToPlayer(player.Client, player.Language.ERROR_OCURRED_DEVS_NOTIFIED);
                    return;
                }

                EWeaponComponent? oldComponentWithSameCategory = null;
                var weaponComponentWithSameCategory = await dbContext.PlayerWeaponComponents
                    .Include(c => c.Component)
                    .FirstOrDefaultAsync(c => c.PlayerId == player.Entity.Id && c.Component.Category == component.Category);
                if (weaponComponentWithSameCategory.ComponentHash == weaponComponentHash)
                {
                    return;
                }

                if (weaponComponentWithSameCategory != null)
                {
                    oldComponentWithSameCategory = weaponComponentWithSameCategory.ComponentHash;
                    dbContext.PlayerWeaponComponents.Remove(weaponComponentWithSameCategory);
                }
                var weaponComponent = new PlayerWeaponComponents { PlayerId = player.Entity.Id, ComponentHash = weaponComponentHash, WeaponHash = weaponHash };
                dbContext.PlayerWeaponComponents.Add(weaponComponent);
                await dbContext.SaveChangesAsync();

                WeaponSyncData weaponUpgradesData;
                if (player.WeaponUpgradesDatas.ContainsKey((uint)weaponHash))
                    weaponUpgradesData = player.WeaponUpgradesDatas[(uint)weaponHash];
                else
                    weaponUpgradesData = new WeaponSyncData { WeaponHash = (uint)weaponHash };

                if (oldComponentWithSameCategory.HasValue && weaponUpgradesData.ComponentHashes.Contains((uint)oldComponentWithSameCategory.Value))
                    weaponUpgradesData.ComponentHashes.Remove((uint)oldComponentWithSameCategory.Value);
                weaponUpgradesData.ComponentHashes.Add((uint)weaponComponentHash);

                player.WeaponUpgradesDatasJson[(uint)weaponHash] = JsonConvert.SerializeObject(weaponUpgradesData);
                player.WeaponUpgradesDatasJsonComplete = JsonConvert.SerializeObject(player.WeaponUpgradesDatas);

                if (player.CurrentLobby is FightLobby lobby && (uint)player.Client.CurrentWeapon == (uint)weaponHash)
                    lobby.SyncPlayerWeaponUpgradesToAll(player, (uint)weaponHash);
            } 
            catch (Exception ex)
            {
                ErrorLogsManager.Log($"Buying weapon component {weaponComponentHash.ToString()} failed. Exception: " + ex.GetBaseException().ToString(), 
                    ex.StackTrace ?? Environment.StackTrace, player);
            }
        }

        public static async void BuyTint(TDSPlayer player, EWeaponHash weaponHash, int tintIndex, bool isMk2)
        {
            if (player.Entity is null)
                return;

            try
            {
                //todo: Remove money from player (?)
                using var dbContext = new TDSNewContext();
                var tint = await dbContext.WeaponsTints.FindAsync(tintIndex, isMk2);
                if (tint == null)
                {
                    ErrorLogsManager.Log($"{player.Client.Name} with ID {player.Entity?.Id ?? '?'} tried to buy weapon-tint {tintIndex} (isMk2: {isMk2})"
                                            + "  but the tint doesn't exist in DB.", Environment.StackTrace, player);
                    NAPI.Notification.SendNotificationToPlayer(player.Client, player.Language.ERROR_OCURRED_DEVS_NOTIFIED);
                    return;
                }

                var weaponTintForSameWeapon = await dbContext.PlayerWeaponTints
                    .Include(c => c.Tint)
                    .FirstOrDefaultAsync(c => c.PlayerId == player.Entity.Id && c.WeaponHash == weaponHash);
                if (weaponTintForSameWeapon.TintId == tintIndex)
                {
                    return;
                }

                int oldTint = -1;
                if (weaponTintForSameWeapon != null)
                {
                    oldTint = weaponTintForSameWeapon.TintId;
                    dbContext.PlayerWeaponTints.Remove(weaponTintForSameWeapon);
                }
                var playerWeaponTint = new PlayerWeaponTints { PlayerId = player.Entity.Id, TintId = tintIndex, IsMK2 = isMk2, WeaponHash = weaponHash };
                dbContext.PlayerWeaponTints.Add(playerWeaponTint);
                await dbContext.SaveChangesAsync();

                WeaponSyncData weaponUpgradesData;
                if (player.WeaponUpgradesDatas.ContainsKey((uint)weaponHash))
                    weaponUpgradesData = player.WeaponUpgradesDatas[(uint)weaponHash];
                else
                    weaponUpgradesData = new WeaponSyncData { WeaponHash = (uint)weaponHash };

                weaponUpgradesData.TintIndex = tintIndex; 

                player.WeaponUpgradesDatasJson[(uint)weaponHash] = JsonConvert.SerializeObject(weaponUpgradesData);
                player.WeaponUpgradesDatasJsonComplete = JsonConvert.SerializeObject(player.WeaponUpgradesDatas);

                if (player.CurrentLobby is FightLobby lobby && (uint)player.Client.CurrentWeapon == (uint)weaponHash)
                    lobby.SyncPlayerWeaponUpgradesToAll(player, (uint)weaponHash);
            }
            catch (Exception ex)
            {
                ErrorLogsManager.Log($"Buying weapon tint {tintIndex} (isMk2: {isMk2.ToString()}) failed. Exception: " + ex.GetBaseException().ToString(),
                    ex.StackTrace ?? Environment.StackTrace, player);
            }
        }
    }
}
