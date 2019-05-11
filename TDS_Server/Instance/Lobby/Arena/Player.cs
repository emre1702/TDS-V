using GTANetworkAPI;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Common.Dto.Map;
using TDS_Common.Instance.Utility;
using TDS_Server.Dto;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;
using TDS_Server.Manager.Helper;
using TDS_Server.Manager.Utility;

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

            string mapname = _currentMap == null ? "-" : _currentMap.Info.Name;
            NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.SyncCurrentMapName, mapname);

            return true;
        }

        public override void RemovePlayer(TDSPlayer character)
        {
            if (character.Lifes > 0)
            {
                RemovePlayerFromAlive(character);
                PlayerCantBeSpectatedAnymore(character);
                DmgSys.CheckLastHitter(character, out TDSPlayer? killercharacter);

                DeathInfoSync(character, killercharacter, (uint)WeaponHash.Unarmed);
                character.KillingSpree = 0;
            }
            else
                RemoveAsSpectator(character);
            if (planter == character)
                planter = null;
            base.RemovePlayer(character);
            RoundCheckForEnoughAlive();
        }

        private void SetPlayerReadyForRound(TDSPlayer character, bool freeze = true)
        {
            Client player = character.Client;
            if (character.Team != null && !character.Team.IsSpectator)
            {
                MapPositionDto? spawndata = GetMapRandomSpawnData(character.Team);
                if (spawndata == null)
                    return;
                NAPI.Player.SpawnPlayer(player, spawndata.ToVector3(), spawndata.Rotation);
                if (character.Team.SpectateablePlayers != null && !character.Team.SpectateablePlayers.Contains(character))
                    character.Team.SpectateablePlayers?.Add(character);
            }
            else
                NAPI.Player.SpawnPlayer(player, SpawnPoint, LobbyEntity.DefaultSpawnRotation);

            RemoveAsSpectator(character);

            Workaround.FreezePlayer(player, freeze);
            GivePlayerWeapons(player);

            if (_removeSpectatorsTimer.ContainsKey(character))
                _removeSpectatorsTimer.Remove(character);

            character.CurrentRoundStats?.Clear();
        }

        private void RemovePlayerFromAlive(TDSPlayer character)
        {
            if (character.Team != null)
            {
                character.Team.AlivePlayers?.Remove(character);
                --character.Team.SyncedTeamData.AmountPlayers.AmountAlive;
            }

            if (bombAtPlayer == character)
            {
                DropBomb();
            }

            _removeSpectatorsTimer[character] = new TDSTimer(() =>
            {
                PlayerCantBeSpectatedAnymore(character);
                SpectateOtherSameTeam(character);
            }, (uint)LobbyEntity.SpawnAgainAfterDeathMs);
        }

        private void StartRoundForPlayer(TDSPlayer player)
        {
            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.RoundStart, player.Team == null || player.Team.IsSpectator);
            if (player.Team != null && !player.Team.IsSpectator)
            {
                SetPlayerAlive(player);
                Workaround.FreezePlayer(player.Client, false);
            }
            player.LastHitter = null;
        }

        private void AddPlayerAsPlayer(TDSPlayer character)
        {
            Team team = GetTeamWithFewestPlayer();
            SetPlayerTeam(character, team);
            if (_currentRoundStatus == ERoundStatus.Countdown)
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
            if (_currentMap != null)
            {
                NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.MapChange, _currentMap.Info.Name,
                    _currentMap.LimitInfo.EdgesJson, JsonConvert.SerializeObject(_currentMap.LimitInfo.Center));
            }

            SendPlayerAmountInFightInfo(player.Client);
            SyncMapVotingOnJoin(player.Client);

            switch (_currentRoundStatus)
            {
                case ERoundStatus.Countdown:
                    NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.CountdownStart, _nextRoundStatusTimer?.RemainingMsToExecute ?? 0);
                    break;

                case ERoundStatus.Round:
                    NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.RoundStart, true, _nextRoundStatusTimer?.RemainingMsToExecute ?? 0);
                    if (bombDetonateTimer != null && bomb != null)
                        NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.BombPlanted, JsonConvert.SerializeObject(bomb.Position), false, bombDetonateTimer.ExecuteAfterMs - bombDetonateTimer.RemainingMsToExecute);
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
            if (player.Team == null || player.Team.AlivePlayers == null)
                return;
            player.Lifes = (sbyte)(LobbyEntity.AmountLifes ?? 0);
            player.Team.AlivePlayers.Add(player);
            var teamamountdata = player.Team.SyncedTeamData.AmountPlayers;
            ++teamamountdata.Amount;
            ++teamamountdata.AmountAlive;
        }
    }
}