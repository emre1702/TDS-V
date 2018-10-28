using GTANetworkAPI;
using TDS.Instance.Player;
using TDS.Instance.Utility;

namespace TDS.Instance.Lobby
{
    partial class Lobby
    { 

        public void OnPlayerSpawn(Character character)
        {
            NAPI.Player.SetPlayerHealth(character.Player, this.entity.StartHealth);
            NAPI.Player.SetPlayerArmor(character.Player, this.entity.StartArmor);
        }

        public void OnPlayerDisconnected(Character character)
        {
            this.RemovePlayer(character);
        }

        
        public uint OnPlayerDeath(Character character)
        {
            if (character.Lifes > 0)
            {
                --character.Lifes;
            }
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
        }
    }
}
