using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Arena
    {
        #region Public Methods

        public override void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon, bool spawnPlayer = true)
        {
            if (player.Lifes > 0)
            {
                if (CurrentRoundStatus == RoundStatus.RoundEnd && player.Team != _currentRoundEndWinnerTeam)
                {
                    DmgSys.OnPlayerDeath(player, killer, weapon);
                    return;
                }

                CurrentGameMode?.OnPlayerDeath(player, killer);

                if (player.Lifes == 1)   // Will be dead
                {
                    RemovePlayerFromAlive(player);
                }
                else   // Will respawn again
                {
                    DeathSpawnTimer[player] = new TDSTimer(() =>
                    {
                        RespawnPlayer(player);
                    }, (uint)Entity.FightSettings.SpawnAgainAfterDeathMs);
                }
            }

            base.OnPlayerDeath(player, killer, weapon, false);
            RoundCheckForEnoughAlive();
        }

        public override void OnPlayerEnterColshape(ITDSColShape shape, ITDSPlayer player)
        {
            base.OnPlayerEnterColshape(shape, player);
            CurrentGameMode?.OnPlayerEnterColshape(shape, player);
        }

        #endregion Public Methods
    }
}
