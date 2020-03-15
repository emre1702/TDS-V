using System.Collections.Generic;
using TDS_Server.Handler.Entities.Player;
using TDS_Shared.Instance;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Lobby
    {
        protected Dictionary<TDSPlayer, TDSTimer> DeathSpawnTimer { get; } = new Dictionary<TDSPlayer, TDSTimer>();

        public virtual void OnPlayerSpawn(TDSPlayer character)
        {
            character.Health = Entity.FightSettings?.StartHealth ?? 100;
            character.Armor = Entity.FightSettings?.StartArmor ?? 100;
        }

        public void OnPlayerDisconnected(TDSPlayer character)
        {
            RemovePlayer(character);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="player"></param>
        /// <param name="killer"></param>
        /// <param name="weapon"></param>
        /// <returns>Time in ms to disapper & spawn again</returns>
        public virtual void OnPlayerDeath(TDSPlayer player, TDSPlayer killer, uint weapon, bool spawnPlayer = true)
        {
            if (spawnPlayer)
            {
                if (DeathSpawnTimer.ContainsKey(player))
                {
                    DeathSpawnTimer[player].Kill();
                    DeathSpawnTimer.Remove(player);
                }

                player.ModPlayer?.Spawn(SpawnPoint.Around(Entity.AroundSpawnPoint), Entity.DefaultSpawnRotation);
            }
        }

        public virtual void OnPlayerEnterColShape(IColShape shape, TDSPlayer character)
        {
        }
    }
}
