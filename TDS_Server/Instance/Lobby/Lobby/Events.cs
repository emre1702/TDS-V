using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Server.Instance.Player;
using TDS_Common.Instance.Utility;

namespace TDS_Server.Instance.Lobby
{
    partial class Lobby
    {
        protected readonly Dictionary<TDSPlayer, Timer> DeathSpawnTimer = new Dictionary<TDSPlayer, Timer>();

        public virtual void OnPlayerSpawn(TDSPlayer character)
        {
            NAPI.Player.SetPlayerHealth(character.Client, LobbyEntity.StartHealth);
            NAPI.Player.SetPlayerArmor(character.Client, LobbyEntity.StartArmor);
        }

        public void OnPlayerDisconnected(TDSPlayer character)
        {
            this.RemovePlayer(character);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="killer"></param>
        /// <param name="weapon"></param>
        /// <returns>Time in ms to disapper & spawn again</returns>
        public virtual void OnPlayerDeath(TDSPlayer character, Client killer, uint weapon, bool spawnPlayer = true)
        {
            if (DeathSpawnTimer.ContainsKey(character))
            {
                DeathSpawnTimer[character].Kill();
                DeathSpawnTimer.Remove(character);
            }
            if (spawnPlayer)
            {
                NAPI.Player.SpawnPlayer(character.Client, SpawnPoint.Around(LobbyEntity.AroundSpawnPoint), LobbyEntity.DefaultSpawnRotation);
            }
        }

        public virtual void OnPlayerEnterColShape(ColShape shape, TDSPlayer character)
        {
        }
    }
}
