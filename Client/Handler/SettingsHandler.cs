using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Data.Interfaces;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Client.Manager.Utility
{
    public class SettingsHandler
    {
        public const int ScreenFadeInTimeAfterSpawn = 2000;
        public const int ScreenFadeOutTimeAfterSpawn = 2000;

        private readonly Dictionary<Language, ILanguage> _languagesDict = new Dictionary<Language, ILanguage>()
        {
            [TDS_Shared.Data.Enums.Language.German] = new German(),
            [TDS_Shared.Data.Enums.Language.English] = new English()
        };

        private Language _languageEnum = TDS_Shared.Data.Enums.Language.English;
        private bool _languageManuallyChanged;

        public Language LanguageEnum
        {
            get => _languageEnum;
            set
            {
                _languageEnum = value;
                _languageManuallyChanged = true;
                Language = _languagesDict[_languageEnum];
                Scoreboard.LoadLanguage();
                Browser.Angular.Main.LoadLanguage(_languageEnum);
                CustomEventManager.SetNewLanguage(Language);
                if (PlayerSettings != null)
                {
                    PlayerSettings.Language = value;
                    RemoteEventsSender.Send(ToServerEvent.LanguageChange, PlayerSettings.Language);
                }
            }
        }

        public ILanguage Language { get; private set; }

        private SyncedServerSettingsDto _syncedServerSettings;
        private SyncedLobbySettingsDto _syncedLobbySettings;

        public SyncedPlayerSettingsDto PlayerSettings;

        public int LobbyId => _syncedLobbySettings.Id;
        public string LobbyName => _syncedLobbySettings != null ? _syncedLobbySettings.Name : "Mainmenu";

        public float DistanceToSpotToPlant => _syncedServerSettings.DistanceToSpotToPlant;
        public float DistanceToSpotToDefuse => _syncedServerSettings.DistanceToSpotToDefuse;

        //public uint BombDefuseTimeMs => syncedLobbySettings.BombDefuseTimeMs.Value;
        //public uint BombPlantTimeMs => syncedLobbySettings.BombPlantTimeMs.Value;
        public int BombDetonateTimeMs => _syncedLobbySettings.BombDetonateTimeMs ?? 0;

        //public uint SpawnAgainAfterDeathMs => syncedLobbySettings.SpawnAgainAfterDeathMs.Value;
        public int CountdownTime => _syncedLobbySettings.CountdownTime ?? 0;

        public int MapChooseTime => _syncedServerSettings.MapChooseTime;
        public int RoundTime => _syncedLobbySettings.RoundTime ?? 0;
        public int RoundEndTime => _syncedServerSettings.RoundEndTime;
        public int MapLimitTime => _syncedLobbySettings.MapLimitTime ?? 0;
        public bool InLobbyWithMaps => _syncedLobbySettings?.InLobbyWithMaps ?? false;
        public MapLimitType MapLimitType => _syncedLobbySettings.MapLimitType ?? MapLimitType.KillAfterTime;
        public int ArenaLobbyId => _syncedServerSettings.ArenaLobbyId;
        public int MapCreatorLobbyId => _syncedServerSettings.MapCreatorLobbyId;

        public float NametagMaxDistance;
        public bool ShowNametagOnlyOnAiming;
        public Color MapBorderColor;
        public int StartHealth = 100;
        public int StartArmor;
        public Color? NametagDeadColor = Color.FromArgb(255, 0, 0, 0);
        public Color NametagHealthEmptyColor = Color.FromArgb(255, 50, 0, 0);
        public Color NametagHealthFullColor = Color.FromArgb(255, 0, 255, 0);
        public Color? NametagArmorEmptyColor = null;
        public Color NametagArmorFullColor = Color.FromArgb(255, 255, 255, 255);

        public bool LoggedIn { get; set; }

        // This is the old MapBorderColor if we changed the color in Angular and not saved it (for display)
        public Color? NotTempMapBorderColor;

        public SettingsHandler(IModAPI modAPI)
        {
            Language = _languagesDict[LanguageEnum];

            RAGE.Nametags.Enabled = false;

            Stats.StatSetInt(Misc.GetHashKey(DPedStat.Flying), 100, false);
            Stats.StatSetInt(Misc.GetHashKey(DPedStat.Lung), 100, false);
            Stats.StatSetInt(Misc.GetHashKey(DPedStat.Shooting), 100, false);
            Stats.StatSetInt(Misc.GetHashKey(DPedStat.Stamina), 100, false);
            Stats.StatSetInt(Misc.GetHashKey(DPedStat.Stealth), 100, false);
            Stats.StatSetInt(Misc.GetHashKey(DPedStat.Strength), 100, false);
            Stats.StatSetInt(Misc.GetHashKey(DPedStat.Wheelie), 100, false);

            RAGE.Game.Player.SetPlayerMaxArmour(Constants.MaxPossibleArmor);
        }

        public void LoadSyncedSettings(SyncedServerSettingsDto loadedSyncedSettings)
        {
            _syncedServerSettings = loadedSyncedSettings;

            NametagMaxDistance = _syncedServerSettings.NametagMaxDistance;
            ShowNametagOnlyOnAiming = _syncedServerSettings.ShowNametagOnlyOnAiming;
        }

        public void LoadUserSettings(SyncedPlayerSettingsDto loadedSyncedSettings)
        {
            if (!_languageManuallyChanged || LanguageEnum == loadedSyncedSettings.Language || PlayerSettings != null)
                LanguageEnum = loadedSyncedSettings.Language;
            else
            {
                loadedSyncedSettings.Language = LanguageEnum;
                RemoteEventsSender.Send(ToServerEvent.LanguageChange, loadedSyncedSettings.Language);
            }

            _languageManuallyChanged = false;
            PlayerSettings = loadedSyncedSettings;

            foreach (var player in RAGE.Elements.Entities.Players.All)
            {
                VoiceManager.SetForPlayer(player);
            }

            MapBorderColor = SharedUtils.GetColorFromHtmlRgba(loadedSyncedSettings.MapBorderColor) ?? MapBorderColor;
            NametagDeadColor = SharedUtils.GetColorFromHtmlRgba(loadedSyncedSettings.NametagDeadColor);
            NametagHealthEmptyColor = SharedUtils.GetColorFromHtmlRgba(loadedSyncedSettings.NametagHealthEmptyColor) ?? NametagHealthEmptyColor;
            NametagHealthFullColor = SharedUtils.GetColorFromHtmlRgba(loadedSyncedSettings.NametagHealthFullColor) ?? NametagHealthFullColor;
            NametagArmorEmptyColor = SharedUtils.GetColorFromHtmlRgba(loadedSyncedSettings.NametagArmorEmptyColor);
            NametagArmorFullColor = SharedUtils.GetColorFromHtmlRgba(loadedSyncedSettings.NametagArmorFullColor) ?? NametagArmorFullColor;

            NotTempMapBorderColor = null;
        }

        public SyncedLobbySettingsDto GetSyncedLobbySettings()
        {
            return _syncedLobbySettings;
        }

        public void LoadSyncedLobbySettings(SyncedLobbySettingsDto loadedSyncedLobbySettings)
        {
            _syncedLobbySettings = loadedSyncedLobbySettings;

            StartHealth = loadedSyncedLobbySettings.StartHealth;
            StartArmor = loadedSyncedLobbySettings.StartArmor;
        }

        public int GetPlantOrDefuseTime(EPlantDefuseStatus status)
        {
            if (status == EPlantDefuseStatus.Defusing)
                return _syncedLobbySettings.BombDefuseTimeMs ?? 0;
            else if (status == EPlantDefuseStatus.Planting)
                return _syncedLobbySettings.BombPlantTimeMs ?? 0;
            return 0;
        }

        public void RevertTempSettings()
        {
            if (NotTempMapBorderColor.HasValue)
            {
                MapBorderColor = NotTempMapBorderColor.Value;
                NotTempMapBorderColor = null;
            }
        }

        public void LoadLanguageFromRAGE()
        {
            int lang = Locale.GetCurrentLanguageId();
            switch (lang)
            {
                case 2: // German
                    LanguageEnum = TDS_Shared.Enum.Language.German;
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
