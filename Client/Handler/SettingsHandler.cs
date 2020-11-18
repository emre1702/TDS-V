using System;
using System.Collections.Generic;
using System.Drawing;
using TDS.Client.Data.Defaults;
using TDS.Client.Data.Enums;
using TDS.Client.Data.Interfaces;
using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Entities.Languages;
using TDS.Client.Handler.Events;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Enums.Userpanel;
using TDS.Shared.Data.Models;
using TDS.Shared.Data.Models.PlayerCommands;
using TDS.Shared.Data.Utility;
using TDS.Shared.Default;

namespace TDS.Client.Handler
{
    public class SettingsHandler : ServiceBase
    {

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

        private readonly BrowserHandler _browserHandler;
        private readonly EventsHandler _eventsHandler;

        private readonly Dictionary<Language, ILanguage> _languagesDict = new Dictionary<Language, ILanguage>()
        {
            [TDS.Shared.Data.Enums.Language.German] = new German(),
            [TDS.Shared.Data.Enums.Language.English] = new English()
        };

        private readonly RemoteEventsSender _remoteEventsSender;

        private Language _languageEnum = TDS.Shared.Data.Enums.Language.English;
        private bool _languageManuallyChanged;
        private SyncedLobbySettings _syncedLobbySettings;
        private SyncedServerSettingsDto _syncedServerSettings;

        public SettingsHandler(LoggingHandler loggingHandler, RemoteEventsSender remoteEventsSender, EventsHandler eventsHandler,
            BrowserHandler browserHandler)
            : base(loggingHandler)
        {
            _remoteEventsSender = remoteEventsSender;
            _eventsHandler = eventsHandler;
            _browserHandler = browserHandler;

            Language = _languagesDict[LanguageEnum];

            eventsHandler.LobbyJoined += LoadSyncedLobbySettings;
            RAGE.Events.Add(FromBrowserEvent.LanguageChange, OnLanguageChangeMethod);
            RAGE.Events.Add(FromBrowserEvent.OnColorSettingChange, OnColorSettingChangeMethod);
            RAGE.Events.Add(ToClientEvent.SyncSettings, OnSyncSettingsMethod);
            RAGE.Events.Add(FromBrowserEvent.SyncRegisterLoginLanguageTexts, SyncRegisterLoginLanguageTexts);
            RAGE.Events.Add(FromBrowserEvent.ReloadPlayerSettings, ReloadTempChangedPlayerSettings);
            RAGE.Events.Add(ToClientEvent.SyncPlayerCommandsSettings, LoadCommandsData);

            RAGE.Nametags.Enabled = false;

            RAGE.Game.Stats.StatSetInt(RAGE.Game.Misc.GetHashKey(PedStat.Flying), 100, false);
            RAGE.Game.Stats.StatSetInt(RAGE.Game.Misc.GetHashKey(PedStat.Lung), 100, false);
            RAGE.Game.Stats.StatSetInt(RAGE.Game.Misc.GetHashKey(PedStat.Shooting), 100, false);
            RAGE.Game.Stats.StatSetInt(RAGE.Game.Misc.GetHashKey(PedStat.Stamina), 100, false);
            RAGE.Game.Stats.StatSetInt(RAGE.Game.Misc.GetHashKey(PedStat.Stealth), 100, false);
            RAGE.Game.Stats.StatSetInt(RAGE.Game.Misc.GetHashKey(PedStat.Strength), 100, false);
            RAGE.Game.Stats.StatSetInt(RAGE.Game.Misc.GetHashKey(PedStat.Wheelie), 100, false);

            //Todo: This line bugged, use Constants.MaxPossibleArmor after bug fix by RAGE
            RAGE.Game.Player.SetPlayerMaxArmour(100);
            LoadLanguageFromRAGE();
        }

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
        public SyncedClientPlayerSettings PlayerSettings { get; private set; }

        //public uint BombDefuseTimeMs => syncedLobbySettings.BombDefuseTimeMs.Value;
        //public uint BombPlantTimeMs => syncedLobbySettings.BombPlantTimeMs.Value;
        public int BombDetonateTimeMs => _syncedLobbySettings.BombDetonateTimeMs ?? 0;

        //public uint SpawnAgainAfterDeathMs => syncedLobbySettings.SpawnAgainAfterDeathMs.Value;
        public int CountdownTime => _syncedLobbySettings.CountdownTime ?? 0;

        public float DistanceToSpotToDefuse => _syncedServerSettings.DistanceToSpotToDefuse;

        public float DistanceToSpotToPlant => _syncedServerSettings.DistanceToSpotToPlant;

        public bool InLobbyWithMaps => _syncedLobbySettings?.InLobbyWithMaps ?? false;
        public int MapChooseTime => _syncedServerSettings.MapChooseTime;
        public int MapLimitTime => _syncedLobbySettings.MapLimitTime ?? 0;
        public MapLimitType MapLimitType => _syncedLobbySettings.MapLimitType ?? MapLimitType.KillAfterTime;
        public int RoundEndTime => _syncedServerSettings.RoundEndTime;
        public int RoundTime => _syncedLobbySettings.RoundTime ?? 0;

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
            CommandsData = Serializer.FromServer<UserpanelPlayerCommandData>((string)args[0]);
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

        public void LoadUserSettings(SyncedPlayerSettings loadedSyncedSettings)
        {
            if (!_languageManuallyChanged || LanguageEnum == loadedSyncedSettings.Client.Language || PlayerSettings != null)
                LanguageEnum = loadedSyncedSettings.Client.Language;
            else
            {
                loadedSyncedSettings.Client.Language = LanguageEnum;
                _remoteEventsSender.Send(ToServerEvent.LanguageChange, loadedSyncedSettings.Client.Language);
            }

            _languageManuallyChanged = false;
            PlayerSettings = loadedSyncedSettings.Client;

            MapBorderColor = SharedUtils.GetColorFromHtmlRgba(loadedSyncedSettings.Client.MapBorderColor) ?? MapBorderColor;
            NametagDeadColor = SharedUtils.GetColorFromHtmlRgba(loadedSyncedSettings.Client.NametagDeadColor);
            NametagHealthEmptyColor = SharedUtils.GetColorFromHtmlRgba(loadedSyncedSettings.Client.NametagHealthEmptyColor) ?? NametagHealthEmptyColor;
            NametagHealthFullColor = SharedUtils.GetColorFromHtmlRgba(loadedSyncedSettings.Client.NametagHealthFullColor) ?? NametagHealthFullColor;
            NametagArmorEmptyColor = SharedUtils.GetColorFromHtmlRgba(loadedSyncedSettings.Client.NametagArmorEmptyColor);
            NametagArmorFullColor = SharedUtils.GetColorFromHtmlRgba(loadedSyncedSettings.Client.NametagArmorFullColor) ?? NametagArmorFullColor;

            NotTempMapBorderColor = null;

            _eventsHandler.OnMapBorderColorChanged(MapBorderColor);

            _browserHandler.Angular.LoadSettings(loadedSyncedSettings.AngularJson);

            _eventsHandler.OnSettingsLoaded(loadedSyncedSettings.Client);
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

        private void LoadLanguageFromRAGE()
        {
            var lang = (LanguageID)RAGE.Game.Locale.GetCurrentLanguageId();
            switch (lang)
            {
                case LanguageID.German:
                    LanguageEnum = TDS.Shared.Data.Enums.Language.German;
                    _languageManuallyChanged = false;
                    break;
            }
        }

        private void OnColorSettingChangeMethod(object[] args)
        {
            var dataSetting = (UserpanelSettingAtClientside)Convert.ToInt32(args[0]);
            var color = (string)args[1];
            
            switch (dataSetting)
            {
                case UserpanelSettingAtClientside.MapBorderColor:
                    MapBorderColor = SharedUtils.GetColorFromHtmlRgba(color) ?? MapBorderColor;
                    _eventsHandler.OnMapBorderColorChanged(MapBorderColor);
                    break;

                case UserpanelSettingAtClientside.NametagDeadColor:
                    NametagDeadColor = SharedUtils.GetColorFromHtmlRgba(color);
                    break;

                case UserpanelSettingAtClientside.NametagHealthEmptyColor:
                    NametagHealthEmptyColor = SharedUtils.GetColorFromHtmlRgba(color) ?? NametagHealthEmptyColor;
                    break;

                case UserpanelSettingAtClientside.NametagHealthFullColor:
                    NametagHealthFullColor = SharedUtils.GetColorFromHtmlRgba(color) ?? NametagHealthFullColor;
                    break;

                case UserpanelSettingAtClientside.NametagArmorEmptyColor:
                    NametagArmorEmptyColor = SharedUtils.GetColorFromHtmlRgba(color);
                    break;

                case UserpanelSettingAtClientside.NametagArmorFullColor:
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
            var json = (string)args[0];
            var settings = Serializer.FromServer<SyncedPlayerSettings>(json);
            LoadUserSettings(settings);
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
