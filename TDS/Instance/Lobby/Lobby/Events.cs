using GTANetworkAPI;
using System.Collections.Generic;
using TDS.Instance.Player;
using TDS.Instance.Utility;

namespace TDS.Instance.Lobby
{
    partial class Lobby
    {
        protected readonly Dictionary<Character, Timer> DeathSpawnTimer = new Dictionary<Character, Timer>();

        public virtual void OnPlayerSpawn(Character character)
        {
            NAPI.Player.SetPlayerHealth(character.Player, LobbyEntity.StartHealth);
            NAPI.Player.SetPlayerArmor(character.Player, LobbyEntity.StartArmor);
        }

        public void OnPlayerDisconnected(Character character)
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
        public virtual void OnPlayerDeath(Character character, Client killer, uint weapon, bool spawnPlayer = true)
        {
            if (DeathSpawnTimer.ContainsKey(character))
            {
                DeathSpawnTimer[character].Kill();
                DeathSpawnTimer.Remove(character);
            }
            if (spawnPlayer)
            {
                NAPI.Player.SpawnPlayer(character.Player, spawnPoint.Around(LobbyEntity.AroundSpawnPoint), LobbyEntity.DefaultSpawnRotation);
            }
        }

        public virtual void OnPlayerEnterColShape(ColShape shape, Character character)
        {
        }
    }
}
