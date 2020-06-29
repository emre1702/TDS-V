﻿using System;
using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Entities.Languages;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Userpanel;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.PlayerCommands;
using TDS_Shared.Data.Utility;
using TDS_Shared.Default;

namespace TDS_Client.Handler
{
    public class SettingsHandler : ServiceBase
    {
        #region Public Fields

        public readonly int ScreenFadeInTimeAfterSpawn = 2000;
        public readonly int ScreenFadeOutTimeAfterSpawn = 2000;

        public Color MapBorderColor;
        public Color? NametagArmorEmptyColor = null;
        public Color NametagArmorFullColor = Color.FromArgb(255, 255, 255, 255);
        public Color? NametagDeadColor = Color.FromArgb(255, 0, 0, 0);
        public Color NametagHealthEmptyColor = Color.FromArgb(255, 50, 0, 0);
        public Color NametagHealthFullColor = Color.FromArgb(255, 0, 255, 0);
        public float NametagMaxDistance;

        // This is the old MapBorderColor if we changed the color in Angular and not saved it (for display)
        public Color? NotTempMapBorderColor;

        public bool ShowNametagOnlyOnAiming;
        public int StartArmor;
        public int StartHealth = 100;

        #endregion Public Fields

        #region Private Fields

        private readonly BrowserHandler _browserHandler;
        private readonly EventsHandler _eventsHandler;

        private readonly Dictionary<Language, ILanguage> _languagesDict = new Dictionary<Language, ILanguage>()
        {
            [TDS_Shared.Data.Enums.Language.German] = new German(),
            [TDS_Shared.Data.Enums.Language.English] = new English()
        };

        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly Serializer _serializer;
        private Language _languageEnum = TDS_Shared.Data.Enums.Language.English;
        private bool _languageManuallyChanged;
        private SyncedLobbySettings _syncedLobbySettings;
        private SyncedServerSettingsDto _syncedServerSettings;

        #endregion Private Fields

        #region Public Constructors

        public SettingsHandler(IModAPI modAPI, LoggingHandler loggingHandler, RemoteEventsSender remoteEventsSender, EventsHandler eventsHandler,
            BrowserHandler browserHandler, Serializer serializer)
            : base(modAPI, loggingHandler)
        {
            _remoteEventsSender = remoteEventsSender;
            _eventsHandler = eventsHandler;
            _browserHandler = browserHandler;
            _serializer = serializer;

            Language = _languagesDict[LanguageEnum];

            eventsHandler.LobbyJoined += LoadSyncedLobbySettings;
            modAPI.Event.Add(FromBrowserEvent.LanguageChange, OnLanguageChangeMethod);
            modAPI.Event.Add(FromBrowserEvent.OnColorSettingChange, OnColorSettingChangeMethod);
            modAPI.Event.Add(ToClientEvent.SyncSettings, OnSyncSettingsMethod);
            modAPI.Event.Add(FromBrowserEvent.SyncRegisterLoginLanguageTexts, SyncRegisterLoginLanguageTexts);
            modAPI.Event.Add(FromBrowserEvent.ReloadPlayerSettings, ReloadTempChangedPlayerSettings);
            modAPI.Event.Add(ToClientEvent.SyncPlayerCommandsSettings, LoadCommandsData);

            modAPI.Nametags.Enabled = false;

            modAPI.Stats.StatSetInt(modAPI.Misc.GetHashKey(PedStat.Flying), 100, false);
            modAPI.Stats.StatSetInt(modAPI.Misc.GetHashKey(PedStat.Lung), 100, false);
            modAPI.Stats.StatSetInt(modAPI.Misc.GetHashKey(PedStat.Shooting), 100, false);
            modAPI.Stats.StatSetInt(modAPI.Misc.GetHashKey(PedStat.Stamina), 100, false);
            modAPI.Stats.StatSetInt(modAPI.Misc.GetHashKey(PedStat.Stealth), 100, false);
            modAPI.Stats.StatSetInt(modAPI.Misc.GetHashKey(PedStat.Strength), 100, false);
            modAPI.Stats.StatSetInt(modAPI.Misc.GetHashKey(PedStat.Wheelie), 100, false);

            modAPI.LocalPlayer.SetMaxArmour(Constants.MaxPossibleArmor);
            LoadLanguageFromRAGE();
        }

        #endregion Public Constructors

        #region Public Properties

        public UserpanelPlayerCommandData CommandsData { get; private set; }
        public ILanguage Language { get; private set; }

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

        public int LobbyId => _syncedLobbySettings.Id;
        public string LobbyName => _syncedLobbySettings != null ? _syncedLobbySettings.Name : "Mainmenu";
        public bool LoggedIn { get; set; }
        public SyncedPlayerSettingsDto PlayerSettings { get; private set; }
        public int ArenaLobbyId => _syncedServerSettings.ArenaLobbyId;

        //public uint BombDefuseTimeMs => syncedLobbySettings.BombDefuseTimeMs.Value;
        //public uint BombPlantTimeMs => syncedLobbySettings.BombPlantTimeMs.Value;
        public int BombDetonateTimeMs => _syncedLobbySettings.BombDetonateTimeMs ?? 0;

        public int CharCreatorLobbyId => _syncedServerSettings.CharCreatorLobbyId;

        //public uint SpawnAgainAfterDeathMs => syncedLobbySettings.SpawnAgainAfterDeathMs.Value;
        public int CountdownTime => _syncedLobbySettings.CountdownTime ?? 0;

        public float DistanceToSpotToDefuse => _syncedServerSettings.DistanceToSpotToDefuse;

        public float DistanceToSpotToPlant => _syncedServerSettings.DistanceToSpotToPlant;

        public bool InLobbyWithMaps => _syncedLobbySettings?.InLobbyWithMaps ?? false;
        public int MapChooseTime => _syncedServerSettings.MapChooseTime;
        public int MapCreatorLobbyId => _syncedServerSettings.MapCreatorLobbyId;
        public int MapLimitTime => _syncedLobbySettings.MapLimitTime ?? 0;
        public MapLimitType MapLimitType => _syncedLobbySettings.MapLimitType ?? MapLimitType.KillAfterTime;
        public int RoundEndTime => _syncedServerSettings.RoundEndTime;
        public int RoundTime => _syncedLobbySettings.RoundTime ?? 0;

        #endregion Public Properties

        #region Public Methods

        public int GetPlantOrDefuseTime(PlantDefuseStatus status)
        {
            if (status == PlantDefuseStatus.Defusing)
                return _syncedLobbySettings.BombDefuseTimeMs ?? 0;
            else if (status == PlantDefuseStatus.Planting)
                return _syncedLobbySettings.BombPlantTimeMs ?? 0;
            return 0;
        }

        public SyncedLobbySettings GetSyncedLobbySettings()
        {
            return _syncedLobbySettings;
        }

        public void LoadCommandsData(object[] args)
        {
            CommandsData = _serializer.FromServer<UserpanelPlayerCommandData>((string)args[0]);
        }

        public void LoadSyncedLobbySettings(SyncedLobbySettings loadedSyncedLobbySettings)
        {
            _syncedLobbySettings = loadedSyncedLobbySettings;

            StartHealth = loadedSyncedLobbySettings.StartHealth;
            StartArmor = loadedSyncedLobbySettings.StartArmor;
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

            _eventsHandler.OnMapBorderColorChanged(MapBorderColor);

            _browserHandler.Angular.LoadChatSettings(loadedSyncedSettings.ChatWidth, loadedSyncedSettings.ChatMaxHeight,
                loadedSyncedSettings.ChatFontSize, loadedSyncedSettings.HideDirtyChat, loadedSyncedSettings.HideChatInfo,
                loadedSyncedSettings.ChatInfoFontSize, loadedSyncedSettings.ChatInfoMoveTimeMs);

            SyncThemeSettings(loadedSyncedSettings);

            _eventsHandler.OnSettingsLoaded(loadedSyncedSettings);
        }

        public void RevertTempSettings()
        {
            if (NotTempMapBorderColor.HasValue)
            {
                MapBorderColor = NotTempMapBorderColor.Value;
                NotTempMapBorderColor = null;
                _eventsHandler.OnMapBorderColorChanged(MapBorderColor);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadLanguageFromRAGE()
        {
            var lang = ModAPI.Locale.GetCurrentLanguageId();
            switch (lang)
            {
                case LanguageID.German:
                    LanguageEnum = TDS_Shared.Data.Enums.Language.German;
                    _languageManuallyChanged = false;
                    break;
            }
        }

        private void OnColorSettingChangeMethod(object[] args)
        {
            string color = (string)args[0];
            UserpanelSettingKey dataSetting = (UserpanelSettingKey)(Convert.ToInt32(args[1]));

            switch (dataSetting)
            {
                case UserpanelSettingKey.MapBorderColor:
                    MapBorderColor = SharedUtils.GetColorFromHtmlRgba(color) ?? MapBorderColor;
                    _eventsHandler.OnMapBorderColorChanged(MapBorderColor);
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

        private void OnLanguageChangeMethod(object[] args)
        {
            var languageID = Convert.ToInt32(args[0]);
            if (!Enum.IsDefined(typeof(Language), languageID))
                return;

            LanguageEnum = (Language)languageID;
        }

        private void OnSyncSettingsMethod(object[] args)
        {
            string json = (string)args[0];
            var settings = _serializer.FromServer<SyncedPlayerSettingsDto>(json);
            LoadUserSettings(settings);
            _browserHandler.Angular.LoadUserpanelData((int)UserpanelLoadDataType.SettingsNormal, json);
        }

        private void ReloadTempChangedPlayerSettings(object[] args)
        {
            var oldMapColor = MapBorderColor;
            MapBorderColor = SharedUtils.GetColorFromHtmlRgba(PlayerSettings.MapBorderColor) ?? MapBorderColor;

            if (oldMapColor != MapBorderColor)
                _eventsHandler.OnMapBorderColorChanged(MapBorderColor);

            NametagDeadColor = SharedUtils.GetColorFromHtmlRgba(PlayerSettings.NametagDeadColor);
            NametagHealthEmptyColor = SharedUtils.GetColorFromHtmlRgba(PlayerSettings.NametagHealthEmptyColor) ?? NametagHealthEmptyColor;
            NametagHealthFullColor = SharedUtils.GetColorFromHtmlRgba(PlayerSettings.NametagHealthFullColor) ?? NametagHealthFullColor;
            NametagArmorEmptyColor = SharedUtils.GetColorFromHtmlRgba(PlayerSettings.NametagArmorEmptyColor);
            NametagArmorFullColor = SharedUtils.GetColorFromHtmlRgba(PlayerSettings.NametagArmorFullColor) ?? NametagArmorFullColor;
        }

        private void SyncRegisterLoginLanguageTexts(object[] args)
        {
            _browserHandler.RegisterLogin.SyncLanguage(Language);
        }

        private void SyncThemeSettings(SyncedPlayerSettingsDto settings)
        {
            var data = new ThemeSettings
            {
                UseDarkTheme = settings.UseDarkTheme,
                ThemeBackgroundAlphaPercentage = settings.ThemeBackgroundAlphaPercentage,
                ThemeMainColor = settings.ThemeMainColor,
                ThemeSecondaryColor = settings.ThemeSecondaryColor,
                ThemeWarnColor = settings.ThemeWarnColor,
                ThemeBackgroundDarkColor = settings.ThemeBackgroundDarkColor,
                ThemeBackgroundLightColor = settings.ThemeBackgroundLightColor
            };
            _browserHandler.Angular.SyncThemeSettings(_serializer.ToBrowser(data));
        }

        #endregion Private Methods

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
