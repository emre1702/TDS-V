using GTANetworkAPI;
using TDS_Common.Instance.Utility;
using TDS_Server.Enums;
using TDS_Server.Instance.PlayerInstance;

namespace TDS_Server.Instance.LobbyInstances
{
    partial class Arena
    {
        public override void OnPlayerEnterColShape(ColShape shape, TDSPlayer character)
        {
            base.OnPlayerEnterColShape(shape, character);
            CurrentGameMode?.OnPlayerEnterColShape(shape, character);
        }

        public override void OnPlayerDeath(TDSPlayer character, TDSPlayer killer, uint weapon, bool spawnPlayer = true)
        {
            if (character.Lifes > 0)
            {
                if (CurrentRoundStatus == ERoundStatus.RoundEnd && character.Team != _currentRoundEndWinnerTeam)
                {
                    DmgSys.OnPlayerDeath(character, killer, weapon);
                    return;
                }

                CurrentGameMode?.OnPlayerDeath(character, killer);

                if (character.Lifes == 1)   // Will be dead
                {
                    RemovePlayerFromAlive(character);
                }
                else   // Will respawn again
                {
                    DeathSpawnTimer[character] = new TDSTimer(() =>
                    {
                        RespawnPlayer(character);
                    }, (uint)LobbyEntity.SpawnAgainAfterDeathMs);
                }
            }

            base.OnPlayerDeath(character, killer, weapon, false);
            RoundCheckForEnoughAlive();
        }
    }
}