using System.Collections.Generic;
using TDS_Client.Enum;
using TDS_Common.Dto.Sync;
using PedBase = RAGE.Elements.PedBase;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Sync
{
    static class Weapon
    {
        private static readonly Dictionary<PedBase, WeaponSyncData> _pedWeaponData = new Dictionary<PedBase, WeaponSyncData>();

        /// <summary>
        /// This method does not handle removing components - don't really think we need this
        /// </summary>
        /// <param name="ped"></param>
        public static void PedStreamedIn(PedBase ped)
        {
            if (!_pedWeaponData.TryGetValue(ped, out WeaponSyncData syncData))
                return;
            ped.GiveWeaponTo(syncData.WeaponHash, -1, false, true);
            foreach (var component in syncData.ComponentHashes)
                ped.GiveWeaponComponentTo(syncData.WeaponHash, component);
            ped.SetWeaponTintIndex(syncData.WeaponHash, syncData.TintIndex);
        }

        public static void Set(PedBase ped, WeaponSyncData data)
        {
            _pedWeaponData[ped] = data;
            if (ped.Exists) 
                PedStreamedIn(ped);
        }

        public static void Set(Dictionary<uint, WeaponSyncData> data)
        {
            foreach (var entry in data)
            {
                if (entry.Value.ComponentHashes.Count > 0)
                {
                    foreach (var component in entry.Value.ComponentHashes)
                        Player.LocalPlayer.GiveWeaponComponentTo(entry.Key, component);
                }
                
                Player.LocalPlayer.SetWeaponTintIndex(entry.Key, entry.Value.TintIndex);
            }
                
        }

        public static void Clear()
        {
            _pedWeaponData.Clear();
        }
    }
}
