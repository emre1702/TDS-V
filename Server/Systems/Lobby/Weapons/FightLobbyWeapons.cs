﻿using GTANetworkAPI;
using System.Collections.Generic;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.Weapons;
using TDS.Server.Database.Entity.LobbyEntities;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.LobbySystem.Weapons
{
    public class FightLobbyWeapons : IFightLobbyWeapons
    {
        protected IFightLobby Lobby { get; }
        protected IFightLobbyEventsHandler Events { get; }

        public FightLobbyWeapons(IFightLobby lobby, IFightLobbyEventsHandler events)
        {
            Lobby = lobby;
            Events = events;

            events.PlayerSpawned += Events_PlayerSpawned;
            events.RemoveAfter += RemoveEvents;
        }

        private void RemoveEvents(IBaseLobby lobby)
        {
            lobby.Events.PlayerSpawned -= Events_PlayerSpawned;
            lobby.Events.RemoveAfter -= RemoveEvents;
        }

        private void Events_PlayerSpawned(ITDSPlayer player)
        {
            NAPI.Task.RunSafe(() => GivePlayerWeapons(player));
        }

        public virtual void GivePlayerWeapons(ITDSPlayer player)
        {
            var lastWeapon = player.Deathmatch.LastWeaponOnHand;
            player.RemoveAllWeapons();
            bool giveLastWeapon = false;
            foreach (var weapon in GetAllWeapons())
            {
                player.GiveWeapon(weapon.Hash, 0);
                player.SetWeaponAmmo(weapon.Hash, weapon.Ammo);
                if (weapon.Hash == lastWeapon)
                    giveLastWeapon = true;
            }
            if (giveLastWeapon)
                player.CurrentWeapon = lastWeapon;
        }

        internal virtual IEnumerable<LobbyWeapons> GetAllWeapons()
            => Lobby.Entity.LobbyWeapons;
    }
}
