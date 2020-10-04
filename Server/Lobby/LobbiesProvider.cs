﻿using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.LobbySystem.Lobbies;
using TDS_Shared.Data.Enums;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem
{
    public class LobbiesProvider : ILobbiesProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public LobbiesProvider(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public TLobby Get<TLobby>(LobbyDb entity) where TLobby : IBaseLobby
        {
            IBaseLobby lobby = typeof(TLobby) switch
            {
                IArena _ => ActivatorUtilities.CreateInstance<Arena>(_serviceProvider, entity),
                ICharCreateLobby _ => ActivatorUtilities.CreateInstance<CharCreateLobby>(_serviceProvider, entity),
                IGangActionLobby _ => ActivatorUtilities.CreateInstance<GangActionLobby>(_serviceProvider, entity),
                IGangLobby _ => ActivatorUtilities.CreateInstance<GangLobby>(_serviceProvider, entity),
                IMainMenu _ => ActivatorUtilities.CreateInstance<MainMenu>(_serviceProvider, entity),
                IMapCreatorLobby _ => ActivatorUtilities.CreateInstance<MapCreatorLobby>(_serviceProvider, entity),

                _ => throw new ArgumentException($"LobbiesProvider doesn't create lobby with Lobby entity for type '{typeof(TLobby).FullName}'.")
            };
            return (TLobby)lobby;
        }

        public TLobby Get<TLobby>(ITDSPlayer owner) where TLobby : IBaseLobby
        {
            IBaseLobby lobby = typeof(TLobby) switch
            {
                ICharCreateLobby _ => ActivatorUtilities.CreateInstance<CharCreateLobby>(_serviceProvider, owner),
                IMapCreatorLobby _ => ActivatorUtilities.CreateInstance<MapCreatorLobby>(_serviceProvider, owner),

                _ => throw new ArgumentException($"LobbiesProvider doesn't create lobby with Player for type '{typeof(TLobby).FullName}'.")
            };
            return (TLobby)lobby;
        }

        public IBaseLobby Get(LobbyType lobbyType, LobbyDb entity)
        {
            IBaseLobby lobby = lobbyType switch
            {
                LobbyType.Arena => ActivatorUtilities.CreateInstance<Arena>(_serviceProvider, entity),
                LobbyType.CharCreateLobby => ActivatorUtilities.CreateInstance<CharCreateLobby>(_serviceProvider, entity),
                LobbyType.GangActionLobby => ActivatorUtilities.CreateInstance<GangActionLobby>(_serviceProvider, entity),
                LobbyType.GangLobby => ActivatorUtilities.CreateInstance<GangLobby>(_serviceProvider, entity),
                LobbyType.MainMenu => ActivatorUtilities.CreateInstance<MainMenu>(_serviceProvider, entity),
                LobbyType.MapCreateLobby => ActivatorUtilities.CreateInstance<MapCreatorLobby>(_serviceProvider, entity),

                _ => throw new ArgumentException($"LobbiesProvider doesn't create lobby with Lobby entity for lobbytype '{lobbyType}'.")
            };
            return lobby;
        }
    }
}
