using RAGE.Game;
using System.Collections.Generic;
using TDS_Client.Default;
using TDS_Client.Enum;
using TDS_Client.Instance.Language;
using TDS_Client.Interface;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Draw;
using TDS_Client.Manager.Lobby;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Common.Enum;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Utility
{
    internal static class Settings
    {
        public const int ScreenFadeInTimeAfterSpawn = 2000;
        public const int ScreenFadeOutTimeAfterSpawn = 2000;

        private static readonly Dictionary<ELanguage, ILanguage> _languagesDict = new Dictionary<ELanguage, ILanguage>()
        {
            [ELanguage.German] = new German(),
            [ELanguage.English] = new English()
        };

        private static ELanguage _languageEnum = ELanguage.English;
        private static bool _languageManuallyChanged;

        public static ELanguage LanguageEnum
        {
            get => _languageEnum;
            set
            {
                _languageEnum = value;
                _languageManuallyChanged = true;
                Language = _languagesDict[_languageEnum];
                Scoreboard.LoadLanguage();
                Angular.LoadLanguage(_languageEnum);
                if (syncedPlayerSettings != null)
                {
                    syncedPlayerSettings.Language = value;
                    EventsSender.Send(DToServerEvent.LanguageChange, syncedPlayerSettings.Language);
                }
            }
        }

        public static ILanguage Language { get; private set; } = _languagesDict[LanguageEnum];
        public static bool Bloodscreen => syncedPlayerSettings.Bloodscreen;
        public static bool Hitsound => syncedPlayerSettings.Hitsound;
        public static bool FloatingDamageInfo => syncedPlayerSettings.FloatingDamageInfo;

        private static SyncedServerSettingsDto syncedServerSettings;
        private static SyncedLobbySettingsDto syncedLobbySettings;
        private static SyncedPlayerSettingsDto syncedPlayerSettings;

        public static int LobbyId => syncedLobbySettings.Id;
        public static string LobbyName => syncedLobbySettings != null ? syncedLobbySettings.Name : "Mainmenu";

        public static float DistanceToSpotToPlant => syncedServerSettings.DistanceToSpotToPlant;
        public static float DistanceToSpotToDefuse => syncedServerSettings.DistanceToSpotToDefuse;

        //public static uint BombDefuseTimeMs => syncedLobbySettings.BombDefuseTimeMs.Value;
        //public static uint BombPlantTimeMs => syncedLobbySettings.BombPlantTimeMs.Value;
        public static int BombDetonateTimeMs => syncedLobbySettings.BombDetonateTimeMs ?? 0;

        //public static uint SpawnAgainAfterDeathMs => syncedLobbySettings.SpawnAgainAfterDeathMs.Value;
        public static int CountdownTime => syncedLobbySettings.CountdownTime ?? 0;

        public static int MapChooseTime => syncedServerSettings.MapChooseTime;
        public static int RoundTime => syncedLobbySettings.RoundTime ?? 0;
        public static int RoundEndTime => syncedServerSettings.RoundEndTime;
        public static int DieAfterOutsideMapLimitTime => syncedLobbySettings.DieAfterOutsideMapLimitTime ?? 0;
        public static bool InLobbyWithMaps => syncedLobbySettings?.InLobbyWithMaps ?? false;

        public static void Load()
        {
            Player.LocalPlayer.SetCanAttackFriendly(false, false);

            Stats.StatSetInt(Misc.GetHashKey(DPedStat.Flying), 100, false);
            Stats.StatSetInt(Misc.GetHashKey(DPedStat.Lung), 100, false);
            Stats.StatSetInt(Misc.GetHashKey(DPedStat.Shooting), 100, false);
            Stats.StatSetInt(Misc.GetHashKey(DPedStat.Stamina), 100, false);
            Stats.StatSetInt(Misc.GetHashKey(DPedStat.Stealth), 100, false);
            Stats.StatSetInt(Misc.GetHashKey(DPedStat.Strength), 100, false);
            Stats.StatSetInt(Misc.GetHashKey(DPedStat.Wheelie), 100, false);
        }

        public static void LoadSyncedSettings(SyncedServerSettingsDto loadedSyncedSettings)
        {
            syncedServerSettings = loadedSyncedSettings;
        }

        public static void LoadUserSettings(SyncedPlayerSettingsDto loadedSyncedSettings)
        {
            if (!_languageManuallyChanged || LanguageEnum == loadedSyncedSettings.Language)
                LanguageEnum = loadedSyncedSettings.Language;
            else
            {
                loadedSyncedSettings.Language = LanguageEnum;
                EventsSender.Send(DToServerEvent.LanguageChange, syncedPlayerSettings.Language);
            }
            syncedPlayerSettings = loadedSyncedSettings;
        }

        public static void LoadSyncedLobbySettings(SyncedLobbySettingsDto loadedSyncedLobbySettings)
        {
            syncedLobbySettings = loadedSyncedLobbySettings;
        }

        public static int GetPlantOrDefuseTime(EPlantDefuseStatus status)
        {
            if (status == EPlantDefuseStatus.Defusing)
                return syncedLobbySettings.BombDefuseTimeMs ?? 0;
            else if (status == EPlantDefuseStatus.Planting)
                return syncedLobbySettings.BombPlantTimeMs ?? 0;
            return 0;
        }

        /*function loadSettings() {
            let savedlang = mp.storage.data.language;
            let savedhitsound = mp.storage.data.hitsound;
            let savedbloodscreen = mp.storage.data.bloodscreen;

            if ( typeof savedlang !== "undefined" )
                settingsdata.language = savedlang;
            else {
                let langnumber = mp.game.invoke( "E7A981467BC975BA", 0 );
                if ( langnumber == 2 )
                    settingsdata.language = "" + Language.GERMAN;
            }

            if ( typeof savedhitsound !== "undefined" )
                settingsdata.hitsound = savedhitsound;

            if ( typeof savedbloodscreen !== "undefined" )
                settingsdata.bloodscreen = savedbloodscreen;
        }

         mp.events.add( "onPlayerSettingChange", ( setting, value ) => {
            switch ( setting as PlayerSetting ) {
                case PlayerSetting.LANGUAGE:
                    settingsdata.language = value;
                    mp.storage.data.language = value;
                    EventsSender.SendCooldown( "onPlayerLanguageChange", value );
                    if ( mainbrowserdata.angularloaded ) {
                        loadOrderNamesInBrowser( JSON.stringify( getLang( "orders" ) ) );
                        mainbrowserdata.angular.call( `syncLanguage('${settingsdata.language}');` );
                    }
                    break;

                case PlayerSetting.HITSOUND:
                    settingsdata.hitsound = value;
                    mp.storage.data.hitsound = value;
                    break;

                case PlayerSetting.BLOODSCREEN:
                    settingsdata.bloodscreen = value;
                    mp.storage.data.blodscreen = value;
                    break;
            }
        } );
        */
    }
}