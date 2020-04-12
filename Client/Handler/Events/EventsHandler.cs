using System;
using TDS_Client.Data.Interfaces;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Models;
using TDS_Client.Handler.MapCreator;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;

namespace TDS_Client.Handler.Events
{
    public class EventsHandler
    {

        public delegate void BoolDelegate(bool boolean);
        public event BoolDelegate CursorToggled;
        public event BoolDelegate InFightStatusChanged;
        public event BoolDelegate FreecamToggled;

        public delegate void EmptyDelegate();
        public event EmptyDelegate SettingsLoaded;
        public event EmptyDelegate MapCleared;
        public event EmptyDelegate MapCreatorObjectDeleted;
        public event EmptyDelegate MapCreatorSyncLatestObjectID;
        public event EmptyDelegate LobbyJoinSelectedTeam;

        public delegate void LanguageChangedDelegate(ILanguage lang, bool beforeLogin);
        public event LanguageChangedDelegate LanguageChanged;

        public delegate void WeaponChangedDelegate(WeaponHash previousWeapon, WeaponHash currentHash);
        public event WeaponChangedDelegate WeaponChanged;

        public delegate void DataChangedDelegate(IPlayer player, PlayerDataKey key, object data);
        public event DataChangedDelegate DataChanged;

        public delegate void LobbyLeftJoinedDelegate(SyncedLobbySettingsDto settings);
        public event LobbyLeftJoinedDelegate LobbyJoined;
        public event LobbyLeftJoinedDelegate LobbyLeft;

        public delegate void MapCreatorObjectDelegate(MapCreatorObject mapCreatorObject);
        public event MapCreatorObjectDelegate MapCreatorSyncObjectDeleted;

        private WeaponHash _lastWeaponHash;

        private readonly IModAPI _modAPI;

        public EventsHandler(IModAPI modAPI)
        {
            _modAPI = modAPI;

            _modAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(OnTick));
        }

        internal void OnCursorToggled(bool visible)
        {
            CursorToggled?.Invoke(visible);
        }

        internal void OnLanguageChanged(ILanguage lang, bool beforeLogin)
        {
            LanguageChanged?.Invoke(lang, beforeLogin);
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

        public void OnLobbyJoined(SyncedLobbySettingsDto newSettings)
        {
            LobbyJoined?.Invoke(newSettings);
        }

        internal void OnMapCreatorSyncLatestObjectID()
        {
            MapCreatorSyncLatestObjectID?.Invoke();
        }

        public void OnLobbyLeft(SyncedLobbySettingsDto oldSettings)
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

        internal void OnLobbyJoinSelectedTeam()
        {
            LobbyJoinSelectedTeam?.Invoke();
        }

        private void OnTick(ulong _)
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
