using System;
using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Entities.Languages;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Utility;
using TDS_Shared.Default;

namespace TDS_Client.Handler
{
    public class SettingsHandler
    {
        public readonly int ScreenFadeInTimeAfterSpawn = 2000;
        public readonly int ScreenFadeOutTimeAfterSpawn = 2000;

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
                bool beforeLogin = PlayerSettings == null;
                _eventsHandler?.OnLanguageChanged(Language, beforeLogin);

                if (!beforeLogin)
                {
                    PlayerSettings.Language = value;
                    _remoteEventsSender.Send(ToServerEvent.LanguageChange, PlayerSettings.Language);
                }
            }
        }

        public ILanguage Language { get; private set; }

        private SyncedServerSettingsDto _syncedServerSettings;
        private SyncedLobbySettings _syncedLobbySettings;

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

        private readonly IModAPI _modAPI;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly EventsHandler _eventsHandler;

        public SettingsHandler(IModAPI modAPI, RemoteEventsSender remoteEventsSender, EventsHandler eventsHandler)
        {
            _modAPI = modAPI;
            _remoteEventsSender = remoteEventsSender;
            _eventsHandler = eventsHandler;

            Language = _languagesDict[LanguageEnum];

            eventsHandler.LobbyJoined += LoadSyncedLobbySettings;
            modAPI.Event.Add(FromBrowserEvent.LanguageChange, OnLanguageChangeMethod);
            modAPI.Event.Add(FromBrowserEvent.OnColorSettingChange, OnColorSettingChangeMethod);


            modAPI.Nametags.Enabled = false;

            modAPI.Stats.StatSetInt(modAPI.Misc.GetHashKey(PedStat.Flying), 100, false);
            modAPI.Stats.StatSetInt(modAPI.Misc.GetHashKey(PedStat.Lung), 100, false);
            modAPI.Stats.StatSetInt(modAPI.Misc.GetHashKey(PedStat.Shooting), 100, false);
            modAPI.Stats.StatSetInt(modAPI.Misc.GetHashKey(PedStat.Stamina), 100, false);
            modAPI.Stats.StatSetInt(modAPI.Misc.GetHashKey(PedStat.Stealth), 100, false);
            modAPI.Stats.StatSetInt(modAPI.Misc.GetHashKey(PedStat.Strength), 100, false);
            modAPI.Stats.StatSetInt(modAPI.Misc.GetHashKey(PedStat.Wheelie), 100, false);

            modAPI.LocalPlayer.SetMaxArmor(Constants.MaxPossibleArmor);
            LoadLanguageFromRAGE();

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
                _remoteEventsSender.Send(ToServerEvent.LanguageChange, loadedSyncedSettings.Language);
            }

            _languageManuallyChanged = false;
            PlayerSettings = loadedSyncedSettings;

            MapBorderColor = SharedUtils.GetColorFromHtmlRgba(loadedSyncedSettings.MapBorderColor) ?? MapBorderColor;
            NametagDeadColor = SharedUtils.GetColorFromHtmlRgba(loadedSyncedSettings.NametagDeadColor);
            NametagHealthEmptyColor = SharedUtils.GetColorFromHtmlRgba(loadedSyncedSettings.NametagHealthEmptyColor) ?? NametagHealthEmptyColor;
            NametagHealthFullColor = SharedUtils.GetColorFromHtmlRgba(loadedSyncedSettings.NametagHealthFullColor) ?? NametagHealthFullColor;
            NametagArmorEmptyColor = SharedUtils.GetColorFromHtmlRgba(loadedSyncedSettings.NametagArmorEmptyColor);
            NametagArmorFullColor = SharedUtils.GetColorFromHtmlRgba(loadedSyncedSettings.NametagArmorFullColor) ?? NametagArmorFullColor;

            NotTempMapBorderColor = null;

            _eventsHandler.OnSettingsLoaded();
        }

        public SyncedLobbySettings GetSyncedLobbySettings()
        {
            return _syncedLobbySettings;
        }

        public void LoadSyncedLobbySettings(SyncedLobbySettings loadedSyncedLobbySettings)
        {
            _syncedLobbySettings = loadedSyncedLobbySettings;

            StartHealth = loadedSyncedLobbySettings.StartHealth;
            StartArmor = loadedSyncedLobbySettings.StartArmor;
        }

        public int GetPlantOrDefuseTime(PlantDefuseStatus status)
        {
            if (status == PlantDefuseStatus.Defusing)
                return _syncedLobbySettings.BombDefuseTimeMs ?? 0;
            else if (status == PlantDefuseStatus.Planting)
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

        private void LoadLanguageFromRAGE()
        {
            var lang = _modAPI.Locale.GetCurrentLanguageId();
            switch (lang)
            {
                case LanguageID.German:
                    LanguageEnum = TDS_Shared.Data.Enums.Language.German;
                    _languageManuallyChanged = false;
                    break;
            }
        }

        private void OnLanguageChangeMethod(object[] args)
        {
            var languageID = Convert.ToInt32(args[0]);
            if (!Enum.IsDefined(typeof(Language), languageID))
                return;

            LanguageEnum = (Language)languageID;
        }

        private void OnColorSettingChangeMethod(object[] args)
        {
            string color = (string)args[0];
            UserpanelSettingKey dataSetting = (UserpanelSettingKey)(Convert.ToInt32(args[1]));

            switch (dataSetting)
            {
                case UserpanelSettingKey.MapBorderColor:
                    MapBorderColor = SharedUtils.GetColorFromHtmlRgba(color) ?? MapBorderColor;
                    break;
                case UserpanelSettingKey.NametagDeadColor:
                    NametagDeadColor = SharedUtils.GetColorFromHtmlRgba(color);
                    break;
                case UserpanelSettingKey.NametagHealthEmptyColor:
                    NametagHealthEmptyColor = SharedUtils.GetColorFromHtmlRgba(color) ?? NametagHealthEmptyColor;
                    break;
                case UserpanelSettingKey.NametagHealthFullColor:
                    NametagHealthFullColor = SharedUtils.GetColorFromHtmlRgba(color) ?? NametagHealthFullColor;
                    break;
                case UserpanelSettingKey.NametagArmorEmptyColor:
                    NametagArmorEmptyColor = SharedUtils.GetColorFromHtmlRgba(color);
                    break;
                case UserpanelSettingKey.NametagArmorFullColor:
                    NametagArmorFullColor = SharedUtils.GetColorFromHtmlRgba(color) ?? NametagArmorFullColor;
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
