﻿using System;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Utility;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler.Events;
using static TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers.IBaseLobbyEventsHandler;
using LobbyDb = TDS.Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS.Server.LobbySystem.EventsHandlers
{
    public class BaseLobbyEventsHandler : IBaseLobbyEventsHandler
    {
        public AsyncTaskEvent<LobbyDb>? Created { get; set; }

        public event LobbyCreatedAfterDelegate? CreatedAfter;

        public AsyncTaskEvent<IBaseLobby>? Remove { get; set; }

        public event LobbyDelegate? RemoveAfter;

        public AsyncValueTaskEvent<(ITDSPlayer Player, int HadLifes)>? PlayerLeft { get; set; }

        public AsyncValueTaskEvent<(ITDSPlayer Player, int HadLifes)>? PlayerLeftAfter { get; set; }

        public AsyncValueTaskEvent<(ITDSPlayer Player, int TeamIndex)>? PlayerJoined { get; set; }
        public AsyncValueTaskEvent<(ITDSPlayer Player, int TeamIndex)>? PlayerJoinedAfter { get; set; }

        public event BanDelegate? NewBan;

        public event PlayerColshapeDelegate? PlayerEnteredColshape;

        public event PlayerDelegate? PlayerSpawned;

        public bool IsRemoved { get; private set; }

        private readonly EventsHandler _eventsHandler;
        protected IBaseLobby Lobby { get; }
        protected ILoggingHandler LoggingHandler { get; }

        public BaseLobbyEventsHandler(IBaseLobby lobby, EventsHandler eventsHandler, ILoggingHandler loggingHandler)
        {
            (Lobby, _eventsHandler, LoggingHandler) = (lobby, eventsHandler, loggingHandler);

            RemoveAfter += RemoveEvents;
        }

        protected virtual void RemoveEvents(IBaseLobby lobby)
        {
        }

        public async Task TriggerCreated(LobbyDb entity)
        {
            try
            {
                var task = Created?.InvokeAsync(entity);
                if (task is { })
                    await task.ConfigureAwait(false);
                CreatedAfter?.Invoke(entity);
                _eventsHandler.OnLobbyCreated(Lobby);
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex);
            }
        }

        public async Task TriggerRemove()
        {
            try
            {
                IsRemoved = true;
                var task = Remove?.InvokeAsync(Lobby);
                if (task is { })
                    await task.ConfigureAwait(false);
                RemoveAfter?.Invoke(Lobby);
                _eventsHandler.OnLobbyRemoved(Lobby);
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex);
            }
        }

        public async ValueTask TriggerPlayerLeft(ITDSPlayer player, int hadLifes)
        {
            try
            {
                var task = PlayerLeft?.InvokeAsync((player, hadLifes));
                if (task.HasValue)
                    await task.Value.ConfigureAwait(false);
                task = PlayerLeftAfter?.InvokeAsync((player, hadLifes));
                if (task.HasValue)
                    await task.Value.ConfigureAwait(false);
                _eventsHandler.OnLobbyLeave(player, Lobby);
                player.Events.TriggerLobbyLeft(Lobby);
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex);
            }
        }

        public async ValueTask TriggerPlayerJoined(ITDSPlayer player, int teamIndex)
        {
            try
            {
                var task = PlayerJoined?.InvokeAsync((player, teamIndex));
                if (task.HasValue)
                    await task.Value.ConfigureAwait(false);
                task = PlayerJoinedAfter?.InvokeAsync((player, teamIndex));
                if (task.HasValue)
                    await task.Value.ConfigureAwait(false);
                _eventsHandler.OnLobbyJoin(player, Lobby);
                player.Events.TriggerLobbyJoined(Lobby);
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex);
            }
        }

        public void TriggerNewBan(PlayerBans ban, ulong? targetDiscordUserId)
        {
            try
            {
                NewBan?.Invoke(ban);
                _eventsHandler.OnNewBan(ban, Lobby.Entity.IsOfficial, targetDiscordUserId);
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex);
            }
        }

        public void TriggerPlayerEnteredColshape(ITDSColshape colshape, ITDSPlayer player)
        {
            try
            {
                PlayerEnteredColshape?.Invoke(colshape, player);
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex);
            }
        }

        public void TriggerPlayerSpawned(ITDSPlayer player)
        {
            try
            {
                PlayerSpawned?.Invoke(player);
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex);
            }
        }
    }
}