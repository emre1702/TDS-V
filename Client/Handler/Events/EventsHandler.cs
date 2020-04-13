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
    public class EventsHandler
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

        private readonly IModAPI _modAPI;
        private readonly RemoteEventsSender _remoteEventsSender;

        public EventsHandler(IModAPI modAPI, RemoteEventsSender remoteEventsSender)
        {
            _modAPI = modAPI;
            _remoteEventsSender = remoteEventsSender;

            _modAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(OnTick));
            _modAPI.Event.Add(ToServerEvent.FromBrowserEvent, OnFromBrowserEventMethod);
        }

        private void OnFromBrowserEventMethod(object[] args)
        {
            if (!_remoteEventsSender.SendFromBrowser(args))
                AngularCooldown?.Invoke();
        }

        internal void OnCursorToggled(bool visible)
        {
            CursorToggled?.Invoke(visible);
        }

        internal void OnLanguageChanged(ILanguage lang, bool beforeLogin)
        {
            LanguageChanged?.Invoke(lang, beforeLogin);
        }

        internal void OnPlayerDied(IPlayer player, int teamIndex, bool willRespawn)
        {
            PlayerDied?.Invoke(player, teamIndex, willRespawn);
        }

        internal void OnLocalPlayerDied()
        {
            LocalPlayerDied?.Invoke();
        }

        internal void OnSettingsLoaded()
        {
            SettingsLoaded?.Invoke();
        }

        internal void OnInFightStatusChanged(bool value)
        {
            InFightStatusChanged?.Invoke(value);
        }

        internal void OnDataChanged(IPlayer player, PlayerDataKey key, object data)
        {
            DataChanged?.Invoke(player, key, data);
        }

        public void OnLobbyJoined(SyncedLobbySettings newSettings)
        {
            LobbyJoined?.Invoke(newSettings);
        }

        internal void OnMapCreatorSyncLatestObjectID()
        {
            MapCreatorSyncLatestObjectID?.Invoke();
        }

        public void OnLobbyLeft(SyncedLobbySettings oldSettings)
        {
            LobbyLeft?.Invoke(oldSettings);
        }

        internal void OnMapCreatorObjectDeleted()
        {
            MapCreatorObjectDeleted?.Invoke();
        }

        internal void OnMapCreatorSyncObjectDeleted(MapCreatorObject mapCreatorObject)
        {
            MapCreatorSyncObjectDeleted?.Invoke(mapCreatorObject);
        }

        internal void OnFreecamToggled(bool toggle)
        {
            FreecamToggled?.Invoke(toggle);
        }

        internal void OnShowScoreboard()
        {
            ShowScoreboard?.Invoke();
        }

        internal void OnHideScoreboard()
        {
            HideScoreboard?.Invoke();
        }

        internal void OnCountdownStarted()
        {
            CountdownStarted?.Invoke();
        }

        internal void OnPlayerJoinedSameLobby(IPlayer player)
        {
            PlayerJoinedSameLobby?.Invoke(player);
        }

        internal void OnPlayerLeftSameLobby(IPlayer player, string name)
        {
            PlayerLeftSameLobby?.Invoke(player, name);
        }

        internal void OnMapChanged()
        {
            MapChanged?.Invoke();
        }

        internal void OnMapCleared()
        {
            MapCleared?.Invoke();
        }

        internal void OnRespawned(bool againInFight)
        {
            Respawned?.Invoke(againInFight);
        }

        internal void OnTeamChanged(string currentTeamName)
        {
            TeamChanged?.Invoke(currentTeamName);
        }

        internal void OnLoggedIn()
        {
            LoggedIn?.Invoke();
        }

        internal void OnRoundStarted(bool isSpectator)
        {
            RoundStarted?.Invoke(isSpectator);
        }

        internal void OnRoundEnded()
        {
            RoundEnded?.Invoke();
        }

        private void OnTick(int _)
        {
            CheckNewWeapon();
        }

        private void CheckNewWeapon()
        {
            var currentWeapon = _modAPI.LocalPlayer.GetSelectedWeapon();
            if (currentWeapon != _lastWeaponHash)
            {
                WeaponChanged?.Invoke(_lastWeaponHash, currentWeapon);
                _lastWeaponHash = currentWeapon;
            }
        }

        
    }
}
