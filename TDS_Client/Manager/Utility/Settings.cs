using RAGE.Game;
using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Default;
using TDS_Client.Enum;
using TDS_Client.Instance.Language;
using TDS_Client.Interface;
using TDS_Client.Manager.Draw;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Common.Enum;
using TDS_Common.Manager.Utility;
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
                Browser.Angular.Main.LoadLanguage(_languageEnum);
                if (PlayerSettings != null)
                {
                    PlayerSettings.Language = value;
                    EventsSender.Send(DToServerEvent.LanguageChange, PlayerSettings.Language);
                }
            }
        }

        public static ILanguage Language { get; private set; } = _languagesDict[LanguageEnum];

        private static SyncedServerSettingsDto _syncedServerSettings;
        private static SyncedLobbySettingsDto _syncedLobbySettings;

        public static SyncedPlayerSettingsDto PlayerSettings;

        public static int LobbyId => _syncedLobbySettings.Id;
        public static string LobbyName => _syncedLobbySettings != null ? _syncedLobbySettings.Name : "Mainmenu";

        public static float DistanceToSpotToPlant => _syncedServerSettings.DistanceToSpotToPlant;
        public static float DistanceToSpotToDefuse => _syncedServerSettings.DistanceToSpotToDefuse;

        //public static uint BombDefuseTimeMs => syncedLobbySettings.BombDefuseTimeMs.Value;
        //public static uint BombPlantTimeMs => syncedLobbySettings.BombPlantTimeMs.Value;
        public static int BombDetonateTimeMs => _syncedLobbySettings.BombDetonateTimeMs ?? 0;

        //public static uint SpawnAgainAfterDeathMs => syncedLobbySettings.SpawnAgainAfterDeathMs.Value;
        public static int CountdownTime => _syncedLobbySettings.CountdownTime ?? 0;

        public static int MapChooseTime => _syncedServerSettings.MapChooseTime;
        public static int RoundTime => _syncedLobbySettings.RoundTime ?? 0;
        public static int RoundEndTime => _syncedServerSettings.RoundEndTime;
        public static int MapLimitTime => _syncedLobbySettings.MapLimitTime ?? 0;
        public static bool InLobbyWithMaps => _syncedLobbySettings?.InLobbyWithMaps ?? false;
        public static EMapLimitType MapLimitType => _syncedLobbySettings.MapLimitType ?? EMapLimitType.KillAfterTime;
        public static int AFKKickAfterSec => _syncedServerSettings.AFKKickAfterSec;
        public static int ArenaLobbyId => _syncedServerSettings.ArenaLobbyId;
        public static int MapCreatorLobbyId => _syncedServerSettings.MapCreatorLobbyId;

        public static float NametagMaxDistance;
        public static bool ShowNametagOnlyOnAiming;
        public static Color MapBorderColor;
        public static int StartHealth = 100;
        public static int StartArmor;
        public static bool LoggedIn { get; set; }

        // This is the old MapBorderColor if we changed the color in Angular and not saved it (for display)
        public static Color? NotTempMapBorderColor;

        public static void Load()
        {
            RAGE.Nametags.Enabled = false;

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
            _syncedServerSettings = loadedSyncedSettings;

            NametagMaxDistance = _syncedServerSettings.NametagMaxDistance;
            ShowNametagOnlyOnAiming = _syncedServerSettings.ShowNametagOnlyOnAiming;
        }

        public static void LoadUserSettings(SyncedPlayerSettingsDto loadedSyncedSettings)
        {
            if (!_languageManuallyChanged || LanguageEnum == loadedSyncedSettings.Language || PlayerSettings != null)
                LanguageEnum = loadedSyncedSettings.Language;
            else
            {
                loadedSyncedSettings.Language = LanguageEnum;
                EventsSender.Send(DToServerEvent.LanguageChange, loadedSyncedSettings.Language);
            }

            _languageManuallyChanged = false;
            PlayerSettings = loadedSyncedSettings;

            foreach (var player in RAGE.Elements.Entities.Players.All)
            {
                VoiceManager.SetForPlayer(player);
            }

            MapBorderColor = CommonUtils.GetColorFromHtmlRgba(loadedSyncedSettings.MapBorderColor);
            NotTempMapBorderColor = null;
        }

        public static SyncedLobbySettingsDto GetSyncedLobbySettings()
        {
            return _syncedLobbySettings;
        }

        public static void LoadSyncedLobbySettings(SyncedLobbySettingsDto loadedSyncedLobbySettings)
        {
            _syncedLobbySettings = loadedSyncedLobbySettings;

            StartHealth = loadedSyncedLobbySettings.StartHealth;
            StartArmor = loadedSyncedLobbySettings.StartArmor;
        }

        public static int GetPlantOrDefuseTime(EPlantDefuseStatus status)
        {
            if (status == EPlantDefuseStatus.Defusing)
                return _syncedLobbySettings.BombDefuseTimeMs ?? 0;
            else if (status == EPlantDefuseStatus.Planting)
                return _syncedLobbySettings.BombPlantTimeMs ?? 0;
            return 0;
        }

        public static void RevertTempSettings()
        {
            if (NotTempMapBorderColor.HasValue)
            {
                MapBorderColor = NotTempMapBorderColor.Value;
                NotTempMapBorderColor = null;
            }
        }

        public static void LoadLanguageFromRAGE()
        {
            int lang = Locale.GetCurrentLanguageId();
            switch (lang)
            {
                case 2: // German
                    LanguageEnum = ELanguage.German;
                    _languageManuallyChanged = false;
                    break;
            }
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
