using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Common.Instance.Utility;
using TDS_Server.Instance.PlayerInstance;

namespace TDS_Server.Instance.LobbyInstances
{
    partial class Lobby
    {
        protected Dictionary<TDSPlayer, TDSTimer> DeathSpawnTimer { get; } = new Dictionary<TDSPlayer, TDSTimer>();

        public virtual void OnPlayerSpawn(TDSPlayer character)
        {
            NAPI.Player.SetPlayerHealth(character.Player, LobbyEntity.StartHealth);
            NAPI.Player.SetPlayerArmor(character.Player, LobbyEntity.StartArmor);
        }

        public void OnPlayerDisconnected(TDSPlayer character)
        {
            RemovePlayer(character);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="character"></param>
        /// <param name="killer"></param>
        /// <param name="weapon"></param>
        /// <returns>Time in ms to disapper & spawn again</returns>
        public virtual void OnPlayerDeath(TDSPlayer character, TDSPlayer killer, uint weapon, bool spawnPlayer = true)
        {
            if (spawnPlayer)
            {
                if (DeathSpawnTimer.ContainsKey(character))
                {
                    DeathSpawnTimer[character].Kill();
                    DeathSpawnTimer.Remove(character);
                }
                NAPI.Player.SpawnPlayer(character.Player, SpawnPoint.Around(LobbyEntity.AroundSpawnPoint), LobbyEntity.DefaultSpawnRotation);
            }
        }

        public virtual void OnPlayerEnterColShape(ColShape shape, TDSPlayer character)
        {
        }
    }
}