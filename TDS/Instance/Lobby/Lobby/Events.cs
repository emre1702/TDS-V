using GTANetworkAPI;
using System.Collections.Generic;
using TDS.Instance.Player;
using TDS.Instance.Utility;

namespace TDS.Instance.Lobby
{
    partial class Lobby
    {
        private Dictionary<Character, Timer> deathSpawnTimer = new Dictionary<Character, Timer>();

        public void OnPlayerSpawn(Character character)
        {
            NAPI.Player.SetPlayerHealth(character.Player, this.entity.StartHealth);
            NAPI.Player.SetPlayerArmor(character.Player, this.entity.StartArmor);
        }

        public void OnPlayerDisconnected(Character character)
        {
            this.RemovePlayer(character);
        }
 
        public virtual uint OnPlayerDeath(Character character, Client killer, uint weapon)
        {
            if (character.Lifes > 0)
            {
                --character.Lifes;
                if (character.Lifes == 0)
                {
                    if (this.deathSpawnTimer.ContainsKey(character))
                        this.deathSpawnTimer[character].Kill();
                    this.deathSpawnTimer[character] = Timer.SetTimer(
                        () =>
                        {
                            NAPI.Player.SpawnPlayer(character.Player, this.spawnPoint, this.entity.DefaultSpawnRotation);
                            this.deathSpawnTimer[character] = null;
                        }
                    , this.entity.DisappearAfterDeathMs);
                }
                return this.entity.DisappearAfterDeathMs;
            } else
            {
                NAPI.Player.SpawnPlayer(character.Player, this.spawnPoint, this.entity.DefaultSpawnRotation);
                return 0;
            }
        }

        public virtual void OnPlayerEnterColShape(ColShape shape, Character character)
        {
        }
    }
}
