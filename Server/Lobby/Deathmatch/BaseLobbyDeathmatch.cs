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

        public BaseLobbyDeathmatch(IBaseLobbyEventsHandler events, IBaseLobby lobby)
        {
            Lobby = lobby;

            events.PlayerLeftAfter += ResetPlayer;
        }

        public virtual Task OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon)
        {
            RemoveAfterDeathSpawnTimer(player);
            return Task.CompletedTask;
        }

        public virtual void OnPlayerSpawned(ITDSPlayer player)
        {
            RemoveAfterDeathSpawnTimer(player);
            player.Health = Lobby.Entity.FightSettings?.StartHealth ?? 100;
            player.Armor = Lobby.Entity.FightSettings?.StartArmor ?? 100;
            player.SetClothes(11, 0, 0);
        }

        protected virtual void ResetPlayer(ITDSPlayer player)
        {
            RemoveAfterDeathSpawnTimer(player);
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
