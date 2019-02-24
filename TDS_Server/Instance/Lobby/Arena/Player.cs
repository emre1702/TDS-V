using GTANetworkAPI;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TDS_Server.Dto;
using TDS_Server.Entity;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Common.Default;
using TDS_Common.Instance.Utility;
using TDS_Common.Dto;
using System.Linq;
using TDS_Server.Instance.Utility;

namespace TDS_Server.Instance.Lobby
{

    partial class Arena
    {

        public override async Task<bool> AddPlayer(TDSPlayer character, uint teamindex)
        {
            if (!await base.AddPlayer(character, teamindex))
                return false;

            character.CurrentRoundStats = new RoundStatsDto(character);
            SpectateOtherSameTeam(character);

            if (teamindex != 0)
                AddPlayerAsPlayer(character);

            SendPlayerRoundInfoOnJoin(character);

            string mapname = currentMap == null ? "-" : currentMap.SyncedData.Name;
            NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.SyncCurrentMapName, mapname);

            return true;
        }

        public override void RemovePlayer(TDSPlayer character)
        {
            if (character.Lifes > 0)
            {
                RemovePlayerFromAlive(character, false);
                PlayerCantBeSpectatedAnymore(character);
                DmgSys.CheckLastHitter(character, out TDSPlayer killercharacter);

                DeathInfoSync(character, killercharacter?.Client, (uint)WeaponHash.Unarmed);
                character.KillingSpree = 0;
            }
            else
                RemoveAsSpectator(character);
            base.RemovePlayer(character);
        }

        private void SetPlayerReadyForRound(TDSPlayer character, bool freeze = true)
        {
            Client player = character.Client;
            if (!character.Team.IsSpectator)
            {
                PositionRotationDto spawndata = GetMapRandomSpawnData(character.Team);
                NAPI.Player.SpawnPlayer(player, spawndata.Position, spawndata.Rotation);
                if (character.Team.SpectateablePlayers != null && !character.Team.SpectateablePlayers.Contains(character))
                    character.Team.SpectateablePlayers?.Add(character);
            }
            else
                NAPI.Player.SpawnPlayer(player, SpawnPoint, LobbyEntity.DefaultSpawnRotation);

            RemoveAsSpectator(character);

            player.Freeze(freeze);
            GivePlayerWeapons(player);

            if (removeSpectatorsTimer.ContainsKey(character))
                removeSpectatorsTimer.Remove(character);

            character.CurrentRoundStats.Clear();
        }

        private void RemovePlayerFromAlive(TDSPlayer character, bool removespectators = true)
        {
            character.Team.AlivePlayers.Remove(character);
            --character.Team.SyncedTeamData.AmountPlayers.AmountAlive;
            if (bombAtPlayer == character)
            {
                DropBomb();
            }

            removeSpectatorsTimer[character] = new TDSTimer(() =>
            {
                PlayerCantBeSpectatedAnymore(character);
                SpectateOtherSameTeam(character);

            }, LobbyEntity.SpawnAgainAfterDeathMs.Value);

            RoundCheckForEnoughAlive();
        }

        private void StartRoundForPlayer(TDSPlayer player)
        {
            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.RoundStart, player.Team.IsSpectator);
            if (!player.Team.IsSpectator)
            {
                SetPlayerAlive(player);
                player.Client.Freeze(false);
            }
            player.LastHitter = null;
        }

        private void AddPlayerAsPlayer(TDSPlayer character)
        {
            Team team = GetTeamWithFewestPlayer();
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
                    CurrentRoundEndBecauseOfPlayer = character;
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
                NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.MapChange, currentMap.SyncedData.Name, 
                    JsonConvert.SerializeObject(currentMap.MapLimits), JsonConvert.SerializeObject(currentMap.MapCenter));
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

        private void SendPlayerAmountInFightInfo(Client player)
        {
            SyncedTeamPlayerAmountDto[] amounts = Teams.Skip(1).Select(t => t.SyncedTeamData).Select(t => t.AmountPlayers).ToArray();
            NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.AmountInFightSync, JsonConvert.SerializeObject(amounts));
        }

        private void SetPlayerAlive(TDSPlayer player)
        {
            player.Lifes = (sbyte)LobbyEntity.AmountLifes.Value;
            player.Team.AlivePlayers.Add(player);
            var teamamountdata = player.Team.SyncedTeamData.AmountPlayers;
            ++teamamountdata.Amount;
            ++teamamountdata.AmountAlive;
        }
    }
}
