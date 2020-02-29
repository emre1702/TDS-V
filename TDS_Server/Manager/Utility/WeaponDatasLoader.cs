#define loadWeaponDatas
#define reloadArenaWeaponDamages
#define reloadArenaWeaponHeadshots

using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using TDS_Server.Dto.WeaponsMeta;
using TDS_Server.Manager.Logs;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Rest;

namespace TDS_Server.Manager.Utility
{
    class WeaponDatasLoader
    {
        public static void LoadData()
        {
            LoadWeaponMetaInfos();
            ReloadArenaWeaponDatas();
        }

        [Conditional("loadWeaponDatas")]
        private static void LoadWeaponMetaInfos()
        {
            Log("Checking if weapon_metas folder exists.");
            if (!Directory.Exists("weapon_metas"))
            {
                Log($"The folder does not exist.{Environment.NewLine}Consider disabling loading the weapon datas or add the directory with meta files into path '{Path.GetDirectoryName("weapon_metas")}'");
                return;
            }

            Log("Found the folder, starting to get the datas.");
            foreach (var filePath in Directory.GetFiles("weapon_metas"))
            {
                Log($"Starting with '{filePath}'.");
                try
                {
                    var xmlSerializer = new XmlSerializer(typeof(WeaponsMetaDto));
                    using XmlReader reader = XmlReader.Create(File.OpenRead(filePath));
                    if (!xmlSerializer.CanDeserialize(reader))
                    {
                        ErrorLogsManager.Log($"Could not deserialize file {filePath}.", Environment.StackTrace);
                        continue;
                    }

                    var weaponsMeta = (WeaponsMetaDto)xmlSerializer.Deserialize(reader);

                    using var dbContext = new TDSDbContext();
                    foreach (var weaponsData in weaponsMeta.Datas)
                    {
                        try
                        {
                            var weaponHash = GetWeaponHash(weaponsData);
                            if (weaponHash is null)
                            {
                                Log($"Weapon hash for '{weaponsData.Name}' is unknown.", LogType.Warning);
                                continue;
                            }

                            var weaponDbInfo = dbContext.Weapons.Where(w => w.Hash == weaponHash).Select(w => new { w.Hash, w.Type }).FirstOrDefault();
                            if (weaponDbInfo is null)
                            {
                                Log($"Weapon entry in DB missing for '{weaponsData.Name}' with hash '{weaponHash.ToString()}' ({(uint)weaponHash.Value})", LogType.Warning);
                                continue;
                            }

                            var weaponDb = new Weapons
                            {
                                Hash = weaponDbInfo.Hash,
                                Type = weaponDbInfo.Type,
                                ClipSize = (int)weaponsData.ClipSize.Value,
                                Damage = weaponsData.Damage.Value,
                                HeadShotDamageModifier = weaponsData.HeadShotDamageModifier.Value,
                                HitLimbsDamageModifier = weaponsData.HitLimbsDamageModifier.Value,
                                MaxHeadShotDistance = weaponsData.MaxHeadShotDistance.Value,
                                MinHeadShotDistance = weaponsData.MinHeadShotDistance.Value,
                                Range = weaponsData.WeaponRange.Value,
                                ReloadTime = weaponsData.ReloadTime.Value,
                                TimeBetweenShots = weaponsData.TimeBetweenShots.Value
                            };
                            dbContext.Update(weaponDb);
                            dbContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Log($"Loading weapon infos of '{weaponsData.Name}' failed." +
                                $"{Environment.StackTrace}Error: {ex.GetBaseException().Message}" +
                                $"{Environment.NewLine}Stacktrace: {(ex.StackTrace ?? Environment.StackTrace)}", LogType.Error);
                        }

                    }
                }
                catch (Exception ex)
                {
                    Log($"Loading weapon infos from file '{filePath}' failed." +
                                $"{Environment.StackTrace}Error: {ex.GetBaseException().Message}" +
                                $"{Environment.NewLine}Stacktrace: {(ex.StackTrace ?? Environment.StackTrace)}", LogType.Error);
                }
            }
        }

        [Conditional("reloadArenaWeaponDamages"), Conditional("reloadArenaWeaponHeadshots")]
        private static void ReloadArenaWeaponDatas()
        {
            using var dbContext = new TDSDbContext();

            #if reloadArenaWeaponDamages
            var dataDict = dbContext.Weapons.AsNoTracking().ToDictionary(w => w.Hash, w => w.Damage);
            #endif

            foreach (var lobbyWeapon in dbContext.LobbyWeapons)
            {
                #if reloadArenaWeaponDamages
                lobbyWeapon.Damage = dataDict[lobbyWeapon.Hash];
                #endif

                #if reloadArenaWeaponHeadshots
                lobbyWeapon.HeadMultiplicator = 1.5f;
                #endif
            }
        }

        private static WeaponHash? GetWeaponHash(WeaponData data)
            => data.Name switch
            {
                "WEAPON_PROXMINE" => WeaponHash.Proximine,
                _ => TryConvertNameToWeaponHash(data.Name)
            };

        private static WeaponHash? TryConvertNameToWeaponHash(string name)
        {
            if (name.Length < "WEAPON_".Length)
                return null;

            string weaponName = name.Substring("WEAPON_".Length);

            if (Enum.TryParse(weaponName, true, out WeaponHash result))
                return result;

            return null;
        }

        private static void Log(string msg, LogType logType = LogType.Info)
        {
            switch (logType)
            {
                case LogType.Info:
                    Console.WriteLine("[WeaponDatasLoader - Info] " + msg);
                    break;
                case LogType.Warning:
                    Console.WriteLine("[WeaponDatasLoader - Warning] " + msg);
                    break;
                case LogType.Error:
                    Console.WriteLine("[WeaponDatasLoader - Error] " + msg);
                    break;
            }
        }

        private enum LogType
        {
            Info,
            Warning,
            Error
        }
    }
}
