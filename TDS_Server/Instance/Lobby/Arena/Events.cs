using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;
using TDS_Common.Instance.Utility;

namespace TDS_Server.Instance.Lobby
{
    partial class Arena
    {
        public override void OnPlayerEnterColShape(ColShape shape, TDSPlayer character)
        {
            base.OnPlayerEnterColShape(shape, character);
            if (lobbyBombTakeCol.ContainsKey(this))
            {
                if (character.Lifes > 0 && character.Team == terroristTeam)
                {
                    TakeBomb(character);
                }
            }
        }

        public override void OnPlayerDeath(TDSPlayer character, Client killer, uint weapon, bool spawnPlayer = true)
        {
            if (character.Lifes > 0)
            {
                if (bombAtPlayer == character)
                    DropBomb();

                if (character.Lifes == 1)   // Will be dead
                {
                    RemovePlayerFromAlive(character);
                }
                else   // Will respawn again
                {
                    DeathSpawnTimer[character] = new Timer(() =>
                    {
                        SetPlayerReadyForRound(character, false);
                    }, LobbyEntity.SpawnAgainAfterDeathMs.Value);
                }
            }

            base.OnPlayerDeath(character, killer, weapon, false);

            
        }
    }
}
