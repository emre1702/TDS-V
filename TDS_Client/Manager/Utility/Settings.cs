using RAGE;
using RAGE.Elements;
using RAGE.Game;
using System.Collections.Generic;
using TDS_Client.Default;
using TDS_Client.Enum;
using TDS_Client.Instance.Language;
using TDS_Client.Interface;
using TDS_Client.Manager.Draw;
using TDS_Client.Manager.Lobby;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Common.Enum;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Utility
{
    static class Settings
    {
        public const int ScreenFadeInTimeAfterSpawn = 2000;
        public const int ScreenFadeOutTimeAfterSpawn = 2000;

        private static readonly Dictionary<ELanguage, ILanguage> languagesDict = new Dictionary<ELanguage, ILanguage>()
        {
            [ELanguage.German] = new German(),
            [ELanguage.English] = new English()
        };

        private static ELanguage languageEnum = ELanguage.English;
        private static bool languageManuallyChanged;

        public static ELanguage LanguageEnum
        {
            get => languageEnum;
            set
            {
                languageEnum = value;
                languageManuallyChanged = true;
                Language = languagesDict[languageEnum];
                Scoreboard.LoadLanguage();
                Team.LoadOrderNames();
                if (syncedPlayerSettings != null)
                {
                    syncedPlayerSettings.Language = (byte)value;
                    EventsSender.Send(DToServerEvent.LanguageChange, syncedPlayerSettings.Language);
                }
            }
        }

        public static ILanguage Language { get; private set; } = languagesDict[LanguageEnum];
        public static bool ShowBloodscreen = true;
        public static bool HitsoundOn = true;

        private static SyncedServerSettingsDto syncedServerSettings;
        private static SyncedLobbySettingsDto syncedLobbySettings;
        private static SyncedPlayerSettingsDto syncedPlayerSettings;

        public static uint LobbyId => syncedLobbySettings.Id;
        public static string LobbyName => syncedLobbySettings != null ? syncedLobbySettings.Name : "Mainmenu";

        public static int DistanceToSpotToPlant => syncedServerSettings.DistanceToSpotToPlant;
        public static int DistanceToSpotToDefuse => syncedServerSettings.DistanceToSpotToDefuse;

        //public static uint BombDefuseTimeMs => syncedLobbySettings.BombDefuseTimeMs.Value;
        //public static uint BombPlantTimeMs => syncedLobbySettings.BombPlantTimeMs.Value;
        public static uint BombDetonateTimeMs => syncedLobbySettings.BombDetonateTimeMs.Value;
        //public static uint SpawnAgainAfterDeathMs => syncedLobbySettings.SpawnAgainAfterDeathMs.Value;
        public static uint CountdownTime => syncedLobbySettings.CountdownTime.Value;
        public static int MapChooseTime => syncedServerSettings.MapChooseTime;
        public static uint RoundTime => syncedLobbySettings.RoundTime.Value;
        public static int RoundEndTime => syncedServerSettings.RoundEndTime;
        public static uint DieAfterOutsideMapLimitTime => syncedLobbySettings.DieAfterOutsideMapLimitTime.Value;
        public static bool InLobbyWithMaps => syncedLobbySettings == null ? syncedLobbySettings.InLobbyWithMaps : false;

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
            if (!languageManuallyChanged || LanguageEnum == (ELanguage)loadedSyncedSettings.Language)
                LanguageEnum = (ELanguage)loadedSyncedSettings.Language;
            else
            {
                loadedSyncedSettings.Language = (byte)LanguageEnum;
                EventsSender.Send(DToServerEvent.LanguageChange, syncedPlayerSettings.Language);
            }
            syncedPlayerSettings = loadedSyncedSettings;
        }

        public static void LoadSyncedLobbySettings(SyncedLobbySettingsDto loadedSyncedLobbySettings)
        {
            syncedLobbySettings = loadedSyncedLobbySettings;
        }

        public static uint GetPlantOrDefuseTime(EPlantDefuseStatus status)
        {
            if (status == EPlantDefuseStatus.Defusing)
                return syncedLobbySettings.BombDefuseTimeMs.Value;
            else if (status == EPlantDefuseStatus.Planting)
                return syncedLobbySettings.BombPlantTimeMs.Value;
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
