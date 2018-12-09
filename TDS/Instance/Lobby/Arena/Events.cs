using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Instance.Player;

namespace TDS.Instance.Lobby
{
    partial class Arena
    {
        public override void OnPlayerEnterColShape(ColShape shape, Character character)
        {
            base.OnPlayerEnterColShape(shape, character);
            /*if (lobbyBombTakeCol.ContainsKey(this))
            {
                if (character.Lifes > 0 && character.Team == terroristTeamID)
                {
                    TakeBomb(character);
                }
            }*/
        }

        public override void OnPlayerDeath(Character character, Client killer, uint weapon, bool spawnPlayer = true)
        {
            if (character.Lifes > 0)
            {
                //if (bombAtPlayer == character)
                //    DropBomb();
                if (character.Lifes == 1)
                {
                    //RemovePlayerFromAlive(character);
                }
            }

            base.OnPlayerDeath(character, killer, weapon);
        }

        public override void OnPlayerSpawn(Character character)
        {
            base.OnPlayerSpawn(character);

            //if (character.Lifes > 0)
                //RespawnPlayerInRound(character);
            //else
                //RespawnPlayerInSpectateMode(character);
        }
    }
}
