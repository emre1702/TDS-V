using GTANetworkAPI;
using TDS_Common.Instance.Utility;
using TDS_Server.Instance.Player;

namespace TDS_Server.Instance.Lobby
{
    partial class Arena
    {
        public override void OnPlayerEnterColShape(ColShape shape, TDSPlayer character)
        {
            base.OnPlayerEnterColShape(shape, character);
            CurrentGameMode?.OnPlayerEnterColShape(shape, character);
        }

        public override void OnPlayerDeath(TDSPlayer character, Client killer, uint weapon, bool spawnPlayer = true)
        {
            if (character.Lifes > 0)
            {
                if (CurrentRoundStatus == Enum.ERoundStatus.RoundEnd && character.Team != _currentRoundEndWinnerTeam)
                {
                    DmgSys.OnPlayerDeath(character, CurrentRoundEndBecauseOfPlayer?.Client, weapon);
                    return;
                }

                CurrentGameMode?.OnPlayerDeath(character);

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