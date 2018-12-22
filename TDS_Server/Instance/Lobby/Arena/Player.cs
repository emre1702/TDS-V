using GTANetworkAPI;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TDS_Server.Default;
using TDS_Server.Dto;
using TDS_Server.Entity;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;
using TDS_Server.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Instance.Utility;

namespace TDS_Server.Instance.Lobby {

    partial class Arena {

        public override async Task<bool> AddPlayer(TDSPlayer character, uint teamindex)
        {
            if (!await base.AddPlayer(character, teamindex))
                return false;

            character.CurrentRoundStats = new RoundStatsDto();
            SpectateOtherSameTeam(character);

            //string mapname = currentMap != null ? currentMap.SyncData.Name : "unknown";
            //NAPI.ClientEvent.TriggerClientEvent(character.Client, DCustomEvent.SyncLobbySettings, spectator, mapname, JsonConvert.SerializeObject(Teams), JsonConvert.SerializeObject(teamColorsList),
            //                    countdownTime, roundTime, bombDetonateTime, bombPlantTime, bombDefuseTime,
            //                    RoundEndTime, true);
#warning Implement after client implementation

            if (teamindex != 0)
                AddPlayerAsPlayer(character);

            SendPlayerRoundInfoOnJoin(character);

            return true;
        }

        public override void RemovePlayer(TDSPlayer character)
        {
            if (character.Lifes > 0)
            {
                RemovePlayerFromAlive(character, false);
                PlayerCantBeSpectatedAnymore(character);
                //Damagesys.CheckLastHitter(character, out Character killercharacter);
                //DeathInfoSync(character.Player, character.Team, killercharacter?.Player, (uint)WeaponHash.Unarmed);
                //
                //Damagesys.PlayerSpree.Remove(character);
            } 
            else
                RemoveAsSpectator(character);
            base.RemovePlayer(character);
        }

        private void SetPlayerReadyForRound(TDSPlayer character, bool freeze = true)
        {
            Client player = character.Client;
            if (!character.Team.IsSpectatorTeam)
            {
                PositionRotationDto spawndata = GetMapRandomSpawnData(character.Team);
                NAPI.Player.SpawnPlayer(player, spawndata.Position, spawndata.Rotation);
            }
            else
                NAPI.Player.SpawnPlayer(player, SpawnPoint, LobbyEntity.DefaultSpawnRotation);

            RemoveAsSpectator(character);
            
            player.Freeze(freeze);
            GivePlayerWeapons(player);

            if (!SpectateablePlayers[character.Team.Index].Contains(character))
                SpectateablePlayers[character.Team.Index].Add(character);

            if (removeSpectatorsTimer.ContainsKey(character))
                removeSpectatorsTimer.Remove(character);

            character.CurrentRoundStats.Clear();
        }

        private void RemovePlayerFromAlive(TDSPlayer character, bool removespectators = true)
        {
            AlivePlayers[character.Team.Index].Remove(character);
            if (bombAtPlayer == character)
            {
                DropBomb();
            }

            removeSpectatorsTimer[character] = new Timer(() =>
            {
                PlayerCantBeSpectatedAnymore(character);
                SpectateOtherSameTeam(character);

            }, LobbyEntity.SpawnAgainAfterDeathMs.Value);

            RoundCheckForEnoughAlive();  
        }

        private void StartRoundForPlayer(TDSPlayer player)
        {
            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.RoundStart, player.Team.IsSpectatorTeam);
            if (!player.Team.IsSpectatorTeam)
            {
                player.Lifes = LobbyEntity.AmountLifes.Value;
                AlivePlayers[player.Team.Index].Add(player);
                player.Client.Freeze(false);
            }
            player.LastHitter = null;
        }

        private void AddPlayerAsPlayer(TDSPlayer character)
        {
            Teams team = GetTeamWithFewestPlayer();
            SetPlayerTeam(character, team);
            if (currentRoundStatus == ERoundStatus.Countdown)
            {
                SetPlayerReadyForRound(character);
            }
            else
            {
                SpectateOtherSameTeam(character);
                int teamsinround = GetTeamAmountStillInRound();
                if (teamsinround < 2)
                {
                    currentRoundEndBecauseOfPlayer = character;
                    SetRoundStatus(ERoundStatus.RoundEnd, ERoundEndReason.NewPlayer);
                }
                else
                {
                    NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.PlayerSpectateMode);
                }
            }
        }

        private void SendPlayerRoundInfoOnJoin(TDSPlayer player)
        {
            if (currentMap != null)
            {
                NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.MapChange, currentMap.SyncedData.Name, JsonConvert.SerializeObject(currentMap.MapLimits), currentMap.MapCenter);
            }

            SendPlayerAmountInFightInfo(player.Client);
            SyncMapVotingOnJoin(player.Client);

            switch (currentRoundStatus)
            {
                case ERoundStatus.Countdown:
                    NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.CountdownStart, nextRoundStatusTimer.RemainingMsToExecute);
                    break;
                case ERoundStatus.Round:
                    NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.RoundStart, true, nextRoundStatusTimer.RemainingMsToExecute);
                    break;
            }
        }


        /*


        


        

        public static void PlayerAmountInFightSync ( Client player, List<uint> amountinteam, List<uint> amountaliveinteam ) {
            NAPI.ClientEvent.TriggerClientEvent ( player, "onClientPlayerAmountInFightSync", JsonConvert.SerializeObject ( amountinteam ), 1, JsonConvert.SerializeObject ( amountaliveinteam ) );
        }

        */

    }
}
