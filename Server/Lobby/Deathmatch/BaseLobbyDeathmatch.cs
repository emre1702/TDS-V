﻿using GTANetworkAPI;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Shared.Core;

namespace TDS_Server.LobbySystem.Deathmatch
{
    public class BaseLobbyDeathmatch : IBaseLobbyDeathmatch
    {
        private readonly Dictionary<ITDSPlayer, TDSTimer> _afterDeathSpawnTimer = new Dictionary<ITDSPlayer, TDSTimer>();
        protected readonly IBaseLobby Lobby;
        protected readonly IBaseLobbyEventsHandler Events;

        public BaseLobbyDeathmatch(IBaseLobby lobby, IBaseLobbyEventsHandler events)
        {
            Lobby = lobby;
            Events = events;

            events.PlayerLeftAfter += ResetPlayer;
            events.RemoveAfter += RemoveEvents;
            events.PlayerSpawned += OnPlayerSpawned;
        }

        protected virtual void RemoveEvents(IBaseLobby lobby)
        {
            if (Events.PlayerLeftAfter is { })
                Events.PlayerLeftAfter -= ResetPlayer;
            Events.RemoveAfter -= RemoveEvents;
            Events.PlayerSpawned -= OnPlayerSpawned;
        }

        public virtual Task OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon)
        {
            RemoveAfterDeathSpawnTimer(player);
            return Task.CompletedTask;
        }

        protected virtual void OnPlayerSpawned(ITDSPlayer player)
        {
            // Add weapons here
            RemoveAfterDeathSpawnTimer(player);
            player.Health = Lobby.Entity.FightSettings?.StartHealth ?? 100;
            player.Armor = Lobby.Entity.FightSettings?.StartArmor ?? 100;
            player.SetClothes(11, 0, 0);
        }

        protected virtual ValueTask ResetPlayer((ITDSPlayer Player, int HadLifes) data)
        {
            RemoveAfterDeathSpawnTimer(data.Player);
            return default;
        }

        private void RemoveAfterDeathSpawnTimer(ITDSPlayer player)
        {
            lock (_afterDeathSpawnTimer)
            {
                if (_afterDeathSpawnTimer.TryGetValue(player, out var timer))
                {
                    timer.Kill();
                    _afterDeathSpawnTimer.Remove(player);
                }
            }
        }
    }
}
