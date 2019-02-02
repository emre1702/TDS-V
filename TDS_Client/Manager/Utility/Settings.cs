using RAGE;
using RAGE.Elements;
using System.Collections.Generic;
using TDS_Client.Enum;
using TDS_Client.Instance.Language;
using TDS_Client.Interface;
using TDS_Client.Manager.Draw;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Common.Enum;

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

        public static ELanguage LanguageEnum
        {
            get => languageEnum;
            set
            {
                languageEnum = value;
                Language = languagesDict[languageEnum];
                Scoreboard.LoadLanguage();
                Events.CallRemote(DToServerEvent.LanguageChange, (byte)LanguageEnum);
            }
        }

        public static ILanguage Language { get; private set; } = languagesDict[LanguageEnum];
        public static bool ShowBloodscreen = true;
        public static bool HitsoundOn = true;

        private static SyncedSettingsDto syncedSettings;
        private static SyncedLobbySettingsDto syncedLobbySettings;

        public static uint LobbyId => syncedLobbySettings.Id;
        public static string LobbyName => syncedLobbySettings != null ? syncedLobbySettings.Name : "Mainmenu";

        public static int DistanceToSpotToPlant => syncedSettings.DistanceToSpotToPlant;
        public static int DistanceToSpotToDefuse => syncedSettings.DistanceToSpotToDefuse;

        //public static uint BombDefuseTimeMs => syncedLobbySettings.BombDefuseTimeMs.Value;
        //public static uint BombPlantTimeMs => syncedLobbySettings.BombPlantTimeMs.Value;
        public static uint BombDetonateTimeMs => syncedLobbySettings.BombDetonateTimeMs.Value;
        //public static uint SpawnAgainAfterDeathMs => syncedLobbySettings.SpawnAgainAfterDeathMs.Value;
        public static uint CountdownTime => syncedLobbySettings.CountdownTime.Value;
        public static int MapChooseTime => syncedSettings.MapChooseTime;
        public static uint RoundTime => syncedLobbySettings.RoundTime.Value;
        public static int RoundEndTime => syncedSettings.RoundEndTime;
        public static uint DieAfterOutsideMapLimitTime => syncedLobbySettings.DieAfterOutsideMapLimitTime.Value;
        public static bool InLobbyWithMaps => syncedLobbySettings.InLobbyWithMaps;

        public static void Load()
        {
            Player.LocalPlayer.SetCanAttackFriendly(false, false);
        }

        public static void LoadSyncedSettings(SyncedSettingsDto loadedSyncedSettings)
        {
            syncedSettings = loadedSyncedSettings;
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
                    callRemoteCooldown( "onPlayerLanguageChange", value );
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
