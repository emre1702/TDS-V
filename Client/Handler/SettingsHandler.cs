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

        

        public float NametagMaxDistance;


        public bool ShowNametagOnlyOnAiming;
        public int StartArmor;
        public int StartHealth = 100;

        #endregion Public Fields

        #region Private Fields

        private readonly BrowserHandler _browserHandler;
        private readonly EventsHandler _eventsHandler;

        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly Serializer _serializer;
        private SyncedLobbySettings _syncedLobbySettings;
        private SyncedServerSettingsDto _syncedServerSettings;

        #endregion Private Fields


        #region Public Properties

        public UserpanelPlayerCommandData CommandsData { get; private set; }

        public int LobbyId => _syncedLobbySettings.Id;
        public string LobbyName => _syncedLobbySettings != null ? _syncedLobbySettings.Name : "Mainmenu";
        public bool LoggedIn { get; set; }
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
        public int GangLobbyLobbyId => _syncedServerSettings.GangLobbyLobbyId;
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

        public void LoadThemeSettings(SyncedPlayerThemeSettings data)
        {
            _browserHandler.Angular.SyncThemeSettings(_serializer.ToBrowser(data));
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
