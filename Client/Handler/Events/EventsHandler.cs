using RAGE;
using System;
using System.Collections.Generic;
using System.Drawing;
using TDS.Client.Data.Abstracts.Entities.GTA;
using TDS.Client.Data.Enums;
using TDS.Client.Data.Interfaces;
using TDS.Client.Handler.MapCreator;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models;
using TDS.Shared.Default;
using static RAGE.Events;

namespace TDS.Client.Handler.Events
{
    public class EventsHandler : ServiceBase
    {
        private readonly RemoteEventsSender _remoteEventsSender;

        private WeaponHash _lastWeaponHash;

        public EventsHandler(LoggingHandler loggingHandler, RemoteEventsSender remoteEventsSender)
            : base(loggingHandler)
        {
            _remoteEventsSender = remoteEventsSender;
            Tick += OnTick;

            Add(ToServerEvent.FromBrowserEvent, OnFromBrowserEventMethod);
        }

        public delegate void BoolDelegate(bool boolean);

        public delegate void ColorDelegate(Color color);

        public delegate void DataChangedDelegate(ITDSPlayer player, PlayerDataKey key, object data);

        public delegate void EmptyDelegate();

        public delegate void LanguageChangedDelegate(ILanguage lang, bool beforeLogin);

        public delegate void LobbyLeftJoinedDelegate(SyncedLobbySettings settings);

        public delegate void MapCreatorObjectDelegate(MapCreatorObject mapCreatorObject);

        public delegate void MapCreatorSyncTeamObjectsDeletedDelegate(int teamNumber);

        public delegate void PlayerAndNameDelegate(ITDSPlayer player, string name);

        public delegate void PlayerDelegate(ITDSPlayer player);

        public delegate void PlayerDiedDelegate(ITDSPlayer player, int teamIndex, bool willRespawn);

        public delegate void RespawnedDelegate(bool inFightAgain);

        public delegate void RoundStartedDelegate(bool isSpectator);

        public delegate void SettingsLoadedDelegate(SyncedClientPlayerSettings settings);

        public delegate void TeamChangedDelegate(string currentTeamName);

        public delegate void WeaponChangedDelegate(WeaponHash previousWeapon, WeaponHash currentHash);

        public event EmptyDelegate AngularCooldown;

        public event BoolDelegate ChatInputToggled;

        public event RoundStartedDelegate CountdownStarted;

        public event BoolDelegate CursorToggled;

        public event BoolDelegate CursorToggleRequested;

        public event DataChangedDelegate DataChanged;

        public event BoolDelegate FreecamToggled;

        public event EmptyDelegate HideScoreboard;

        public event BoolDelegate InFightStatusChanged;

        public event LanguageChangedDelegate LanguageChanged;

        public event LobbyLeftJoinedDelegate LobbyJoined;

        public event LobbyLeftJoinedDelegate LobbyLeft;

        public event EmptyDelegate LocalPlayerDied;

        public event EmptyDelegate LoggedIn;

        public event ColorDelegate MapBorderColorChanged;

        public event EmptyDelegate MapChanged;

        public event EmptyDelegate MapCleared;

        public event EmptyDelegate MapCreatorObjectDeleted;

        public event EmptyDelegate MapCreatorSyncLatestObjectID;

        public event MapCreatorObjectDelegate MapCreatorSyncObjectDeleted;

        public event MapCreatorSyncTeamObjectsDeletedDelegate MapCreatorSyncTeamObjectsDeleted;

        public event PlayerDiedDelegate PlayerDied;

        public event PlayerDelegate PlayerJoinedSameLobby;

        public event PlayerAndNameDelegate PlayerLeftSameLobby;

        public event RespawnedDelegate Respawned;

        public event RoundStartedDelegate RoundEnded;

        public event RoundStartedDelegate RoundStarted;

        public event SettingsLoadedDelegate SettingsLoaded;

        public event EmptyDelegate ShowScoreboard;

        public event EmptyDelegate Spawn;

        public event TeamChangedDelegate TeamChanged;

        public event WeaponChangedDelegate WeaponChanged;

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

        internal void OnChatInputToggled(bool value)
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnChatInputToggled");
                ChatInputToggled?.Invoke(value);
                Logging.LogInfo("", "EventsHandler.OnChatInputToggled", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        internal void OnCountdownStarted(bool isSpectator)
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnCountdownStarted");
                CountdownStarted?.Invoke(isSpectator);
                Logging.LogInfo("", "EventsHandler.OnCountdownStarted", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
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

        internal void OnCursorToggleRequested(bool value)
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnChatInputToggleRequested");
                CursorToggleRequested?.Invoke(value);
                Logging.LogInfo("", "EventsHandler.OnChatInputToggleRequested", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        internal void OnDataChanged(ITDSPlayer player, PlayerDataKey key, object data)
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

        internal void OnMapBorderColorChanged(Color color)
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnMapBorderColorChanged");
                MapBorderColorChanged?.Invoke(color);
                Logging.LogInfo("", "EventsHandler.OnMapBorderColorChanged", true);
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

        internal void OnMapCreatorSyncTeamObjectsDeleted(int teamNumber)
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnMapCreatorSyncTeamObjectsDeleted");
                MapCreatorSyncTeamObjectsDeleted?.Invoke(teamNumber);
                Logging.LogInfo("", "EventsHandler.OnMapCreatorSyncTeamObjectsDeleted", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        internal void OnPlayerDied(ITDSPlayer player, int teamIndex, bool willRespawn)
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

        internal void OnPlayerJoinedSameLobby(ITDSPlayer player)
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

        internal void OnPlayerLeftSameLobby(ITDSPlayer player, string name)
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

        internal void OnRoundEnded(bool isSpectator)
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnRoundEnded");
                RoundEnded?.Invoke(isSpectator);
                Logging.LogInfo("", "EventsHandler.OnRoundEnded", true);
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

        internal void OnSettingsLoaded(SyncedClientPlayerSettings loadedSyncedSettings)
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnSettingsLoaded");
                SettingsLoaded?.Invoke(loadedSyncedSettings);
                Logging.LogInfo("", "EventsHandler.OnSettingsLoaded", true);
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

        internal void OnSpawn()
        {
            try
            {
                Logging.LogInfo("", "EventsHandler.OnSpawn");
                Spawn?.Invoke();
                Logging.LogInfo("", "EventsHandler.OnSpawn", true);
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

        private void CheckNewWeapon()
        {
            var currentWeapon = (WeaponHash)RAGE.Elements.Player.LocalPlayer.GetSelectedWeapon();
            if (currentWeapon != _lastWeaponHash)
            {
                WeaponChanged?.Invoke(_lastWeaponHash, currentWeapon);
                _lastWeaponHash = currentWeapon;
            }
        }

        private void OnFromBrowserEventMethod(object[] args)
        {
            if (!_remoteEventsSender.SendFromBrowser(args))
                AngularCooldown?.Invoke();
        }

        private void OnTick(List<TickNametagData> _)
        {
            CheckNewWeapon();
        }
    }
}
