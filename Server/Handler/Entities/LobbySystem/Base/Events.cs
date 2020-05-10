using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.ColShape;
using TDS_Server.Handler.Entities.Player;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Lobby
    {
        protected Dictionary<ITDSPlayer, TDSTimer> DeathSpawnTimer { get; } = new Dictionary<ITDSPlayer, TDSTimer>();

        public virtual void OnPlayerSpawn(ITDSPlayer player)
        {
            player.Health = Entity.FightSettings?.StartHealth ?? 100;
            player.Armor = Entity.FightSettings?.StartArmor ?? 100;
            player.ModPlayer?.SetClothes(11, 0, 0);
        }

        public async void OnPlayerLoggedOut(ITDSPlayer character)
        {
            await RemovePlayer(character);
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

        public virtual void OnPlayerEnterColshape(IColShape colshape, ITDSPlayer player)
        {
        }
    }
}
