using System;
using TDS_Client.Data.Interfaces;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Models;
using TDS_Client.Handler.MapCreator;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Events
{
    public class EventsHandler : ServiceBase
    {

        public delegate void EmptyDelegate();
        public event EmptyDelegate SettingsLoaded;
        public event EmptyDelegate MapCleared;
        public event EmptyDelegate MapCreatorObjectDeleted;
        public event EmptyDelegate MapCreatorSyncLatestObjectID;
        public event EmptyDelegate ShowScoreboard;
        public event EmptyDelegate HideScoreboard;
        public event EmptyDelegate AngularCooldown;
        public event EmptyDelegate CountdownStarted;
        public event EmptyDelegate LocalPlayerDied;
        public event EmptyDelegate MapChanged;
        public event EmptyDelegate LoggedIn;
        public event EmptyDelegate RoundEnded;

        public delegate void BoolDelegate(bool boolean);
        public event BoolDelegate CursorToggled;
        public event BoolDelegate InFightStatusChanged;
        public event BoolDelegate FreecamToggled;

        public delegate void LanguageChangedDelegate(ILanguage lang, bool beforeLogin);
        public event LanguageChangedDelegate LanguageChanged;

        public delegate void WeaponChangedDelegate(WeaponHash previousWeapon, WeaponHash currentHash);
        public event WeaponChangedDelegate WeaponChanged;

        public delegate void DataChangedDelegate(IPlayer player, PlayerDataKey key, object data);
        public event DataChangedDelegate DataChanged;

        public delegate void LobbyLeftJoinedDelegate(SyncedLobbySettings settings);
        public event LobbyLeftJoinedDelegate LobbyJoined;
        public event LobbyLeftJoinedDelegate LobbyLeft;

        public delegate void MapCreatorObjectDelegate(MapCreatorObject mapCreatorObject);
        public event MapCreatorObjectDelegate MapCreatorSyncObjectDeleted;

        public delegate void PlayerDiedDelegate(IPlayer player, int teamIndex, bool willRespawn);
        public event PlayerDiedDelegate PlayerDied;

        public delegate void PlayerDelegate(IPlayer player);
        public event PlayerDelegate PlayerJoinedSameLobby;

        public delegate void PlayerAndNameDelegate(IPlayer player, string name);
        public event PlayerAndNameDelegate PlayerLeftSameLobby;

        public delegate void RespawnedDelegate(bool inFightAgain);
        public event RespawnedDelegate Respawned;

        public delegate void TeamChangedDelegate(string currentTeamName);
        public event TeamChangedDelegate TeamChanged;

        public delegate void RoundStartedDelegate(bool isSpectator);
        public event RoundStartedDelegate RoundStarted;

        private WeaponHash _lastWeaponHash;

        private readonly RemoteEventsSender _remoteEventsSender;

        public EventsHandler(IModAPI modAPI, LoggingHandler loggingHandler, RemoteEventsSender remoteEventsSender)
            : base(modAPI, loggingHandler)
        {
            _remoteEventsSender = remoteEventsSender;

            ModAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(OnTick));
            ModAPI.Event.Add(ToServerEvent.FromBrowserEvent, OnFromBrowserEventMethod);
        }

        private void OnFromBrowserEventMethod(object[] args)
        {
            if (!_remoteEventsSender.SendFromBrowser(args))
                AngularCooldown?.Invoke();
        }

        internal void OnCursorToggled(bool visible)
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnCursorToggled");
                CursorToggled?.Invoke(visible);
                Logging.LogInfo("", "EventsHandler.OnCursorToggled", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
    
        }

        internal void OnLanguageChanged(ILanguage lang, bool beforeLogin)
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnLanguageChanged");
                LanguageChanged?.Invoke(lang, beforeLogin);
                Logging.LogInfo("", "EventsHandler.OnLanguageChanged", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        internal void OnPlayerDied(IPlayer player, int teamIndex, bool willRespawn)
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnPlayerDied");
                PlayerDied?.Invoke(player, teamIndex, willRespawn);
                Logging.LogInfo("", "EventsHandler.OnPlayerDied", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
            
        }

        internal void OnLocalPlayerDied()
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnLocalPlayerDied");
                LocalPlayerDied?.Invoke();
                Logging.LogInfo("", "EventsHandler.OnLocalPlayerDied", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        internal void OnSettingsLoaded()
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnSettingsLoaded");
                SettingsLoaded?.Invoke();
                Logging.LogInfo("", "EventsHandler.OnSettingsLoaded", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        internal void OnInFightStatusChanged(bool value)
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnInFightStatusChanged");
                InFightStatusChanged?.Invoke(value);
                Logging.LogInfo("", "EventsHandler.OnInFightStatusChanged", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        internal void OnDataChanged(IPlayer player, PlayerDataKey key, object data)
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnDataChanged");
                DataChanged?.Invoke(player, key, data);
                Logging.LogInfo("", "EventsHandler.OnDataChanged", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        public void OnLobbyJoined(SyncedLobbySettings newSettings)
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnLobbyJoined");
                LobbyJoined?.Invoke(newSettings);
                Logging.LogInfo("", "EventsHandler.OnLobbyJoined", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        internal void OnMapCreatorSyncLatestObjectID()
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnMapCreatorSyncLatestObjectID");
                MapCreatorSyncLatestObjectID?.Invoke();
                Logging.LogInfo("", "EventsHandler.OnMapCreatorSyncLatestObjectID", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        public void OnLobbyLeft(SyncedLobbySettings oldSettings)
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnLobbyLeft");
                LobbyLeft?.Invoke(oldSettings);
                Logging.LogInfo("", "EventsHandler.OnLobbyLeft", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        internal void OnMapCreatorObjectDeleted()
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnMapCreatorObjectDeleted");
                MapCreatorObjectDeleted?.Invoke();
                Logging.LogInfo("", "EventsHandler.OnMapCreatorObjectDeleted", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        internal void OnMapCreatorSyncObjectDeleted(MapCreatorObject mapCreatorObject)
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnMapCreatorSyncObjectDeleted");
                MapCreatorSyncObjectDeleted?.Invoke(mapCreatorObject);
                Logging.LogInfo("", "EventsHandler.OnMapCreatorSyncObjectDeleted", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        internal void OnFreecamToggled(bool toggle)
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnFreecamToggled");
                FreecamToggled?.Invoke(toggle);
                Logging.LogInfo("", "EventsHandler.OnFreecamToggled", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        internal void OnShowScoreboard()
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnShowScoreboard");
                ShowScoreboard?.Invoke();
                Logging.LogInfo("", "EventsHandler.OnShowScoreboard", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        internal void OnHideScoreboard()
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnHideScoreboard");
                HideScoreboard?.Invoke();
                Logging.LogInfo("", "EventsHandler.OnHideScoreboard", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        internal void OnCountdownStarted()
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnCountdownStarted");
                CountdownStarted?.Invoke();
                Logging.LogInfo("", "EventsHandler.OnCountdownStarted", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        internal void OnPlayerJoinedSameLobby(IPlayer player)
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnPlayerJoinedSameLobby");
                PlayerJoinedSameLobby?.Invoke(player);
                Logging.LogInfo("", "EventsHandler.OnPlayerJoinedSameLobby", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        internal void OnPlayerLeftSameLobby(IPlayer player, string name)
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnPlayerLeftSameLobby");
                PlayerLeftSameLobby?.Invoke(player, name);
                Logging.LogInfo("", "EventsHandler.OnPlayerLeftSameLobby", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        internal void OnMapChanged()
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnMapChanged");
                MapChanged?.Invoke();
                Logging.LogInfo("", "EventsHandler.OnMapChanged", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        internal void OnMapCleared()
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnMapCleared");
                MapCleared?.Invoke();
                Logging.LogInfo("", "EventsHandler.OnMapCleared", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        internal void OnRespawned(bool againInFight)
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnRespawned");
                Respawned?.Invoke(againInFight);
                Logging.LogInfo("", "EventsHandler.OnRespawned", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        internal void OnTeamChanged(string currentTeamName)
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnTeamChanged");
                TeamChanged?.Invoke(currentTeamName);
                Logging.LogInfo("", "EventsHandler.OnTeamChanged", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        internal void OnLoggedIn()
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnLoggedIn");
                LoggedIn?.Invoke();
                Logging.LogInfo("", "EventsHandler.OnLoggedIn", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        internal void OnRoundStarted(bool isSpectator)
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnRoundStarted");
                RoundStarted?.Invoke(isSpectator);
                Logging.LogInfo("", "EventsHandler.OnRoundStarted", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        internal void OnRoundEnded()
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnRoundEnded");
                RoundEnded?.Invoke();
                Logging.LogInfo("", "EventsHandler.OnRoundEnded", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void OnTick(int _)
        {
            CheckNewWeapon();
        }

        private void CheckNewWeapon()
        {
            var currentWeapon = ModAPI.LocalPlayer.GetSelectedWeapon();
            if (currentWeapon != _lastWeaponHash)
            {
                WeaponChanged?.Invoke(_lastWeaponHash, currentWeapon);
                _lastWeaponHash = currentWeapon;
            }
        }

        
    }
}
