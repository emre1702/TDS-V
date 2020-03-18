using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.ColShape;
using TDS_Server.Handler.Entities.Player;
using TDS_Shared.Instance;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Lobby
    {
        protected Dictionary<ITDSPlayer, TDSTimer> DeathSpawnTimer { get; } = new Dictionary<ITDSPlayer, TDSTimer>();

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
        public virtual void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon, bool spawnPlayer = true)
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

        public virtual void OnPlayerEnterColShape(IColShape shape, ITDSPlayer character)
        {
        }
    }
}
