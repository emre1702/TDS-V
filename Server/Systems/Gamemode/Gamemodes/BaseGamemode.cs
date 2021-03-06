﻿using System;
using System.Threading.Tasks;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.GamemodesSystem.Deathmatch;
using TDS.Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS.Server.Data.Interfaces.GamemodesSystem.MapHandler;
using TDS.Server.Data.Interfaces.GamemodesSystem.Players;
using TDS.Server.Data.Interfaces.GamemodesSystem.Rounds;
using TDS.Server.Data.Interfaces.GamemodesSystem.Specials;
using TDS.Server.Data.Interfaces.GamemodesSystem.Teams;
using TDS.Server.Data.Interfaces.GamemodesSystem.Weapons;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Models.Map;
using TDS.Server.GamemodesSystem.Deathmatch;
using TDS.Server.GamemodesSystem.DependenciesModels;
using TDS.Server.GamemodesSystem.MapHandlers;
using TDS.Server.GamemodesSystem.Players;
using TDS.Server.GamemodesSystem.Rounds;
using TDS.Server.GamemodesSystem.Specials;
using TDS.Server.GamemodesSystem.Teams;
using TDS.Server.GamemodesSystem.Weapons;

namespace TDS.Server.GamemodesSystem.Gamemodes
{
    public abstract class BaseGamemode : IBaseGamemode
    {
        protected IRoundFightLobby Lobby { get; private set; }
        protected MapDto Map { get; private set; }

        public IBaseGamemodeDeathmatch Deathmatch => _deathmatch;
        public IBaseGamemodeMapHandler MapHandler => _mapHandler;
        public IBaseGamemodePlayers Players => _players;
        public IBaseGamemodeRounds Rounds => _rounds;
        public IBaseGamemodeSpecials Specials => _specials;
        public IBaseGamemodeTeams Teams => _teams;
        public IBaseGamemodeWeapons Weapons => _weapons;

        protected IServiceProvider ServiceProvider { get; }

        private BaseGamemodeDeathmatch _deathmatch;
        private BaseGamemodeMapHandler _mapHandler;
        private BaseGamemodePlayers _players;
        private BaseGamemodeRounds _rounds;
        private BaseGamemodeSpecials _specials;
        private BaseGamemodeTeams _teams;
        private BaseGamemodeWeapons _weapons;

        public ISettingsHandler SettingsHandler { get; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        protected BaseGamemode(ISettingsHandler settingsHandler, IServiceProvider serviceProvider)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            SettingsHandler = settingsHandler;
            ServiceProvider = serviceProvider;
        }

        public void Init(IRoundFightLobby lobby, MapDto map)
        {
            Lobby = lobby;
            Map = map;

            InitDependencies();
            AddEvents();
        }

        protected virtual void InitDependencies(BaseGamemodeDependencies? d = null)
        {
            d ??= new BaseGamemodeDependencies();

            d.Deathmatch ??= new BaseGamemodeDeathmatch();
            d.MapHandler ??= new BaseGamemodeMapHandler();
            d.Players ??= new BaseGamemodePlayers();
            d.Rounds ??= new BaseGamemodeRounds();
            d.Specials ??= new BaseGamemodeSpecials(Lobby);
            d.Teams ??= new BaseGamemodeTeams();
            d.Weapons ??= new BaseGamemodeWeapons();

            _deathmatch = d.Deathmatch;
            _mapHandler = d.MapHandler;
            _players = d.Players;
            _rounds = d.Rounds;
            _specials = d.Specials;
            _teams = d.Teams;
            _weapons = d.Weapons;
        }

        protected virtual void AddEvents()
        {
            Lobby.Events.RoundClear += RoundClear;

            _deathmatch.AddEvents(Lobby.Events);
            _mapHandler.AddEvents(Lobby.Events);
            _players.AddEvents(Lobby.Events);
            _rounds.AddEvents(Lobby.Events);
            _specials.AddEvents(Lobby.Events);
            _weapons.AddEvents(Lobby.Events);
        }

        protected virtual void RemoveEvents()
        {
            if (Lobby.Events.RoundClear is { })
                Lobby.Events.RoundClear -= RoundClear;

            _deathmatch.RemoveEvents(Lobby.Events);
            _mapHandler.RemoveEvents(Lobby.Events);
            _players.RemoveEvents(Lobby.Events);
            _rounds.RemoveEvents(Lobby.Events);
            _specials.RemoveEvents(Lobby.Events);
            _weapons.RemoveEvents(Lobby.Events);
        }

        private ValueTask RoundClear()
        {
            RemoveEvents();
            return default;
        }
    }
}
