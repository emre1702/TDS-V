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
using TDS_Server.Manager.Sync;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Instance.Lobby
{
    partial class Arena
    {
        public override async Task<bool> AddPlayer(TDSPlayer player, uint? teamindex = null)
        {
            if (!await base.AddPlayer(player, 0))
                return false;
            SpectateOtherAllTeams(player);
            SendPlayerRoundInfoOnJoin(player);

            TeamChoiceMenuSync.AddPlayer(player, this);

            return true;
        }

        public void ChooseTeam(TDSPlayer player, int teamIndex)
        {
            player.CurrentRoundStats = new RoundStatsDto(player);

            if (teamIndex != 0)
            {
                SpectateOtherSameTeam(player);
                AddPlayerAsPlayer(player, teamIndex);
            }
                
            TeamChoiceMenuSync.RemovePlayer(player);
        }

        public override void RemovePlayer(TDSPlayer player)
        {
            if (player.Lifes > 0)
            {
                RemovePlayerFromAlive(player);
                PlayerCantBeSpectatedAnymore(player);
                DmgSys.CheckLastHitter(player, out TDSPlayer? killercharacter);

                DeathInfoSync(player, killercharacter, (uint)WeaponHash.Unarmed);
            }
            else
            {
                SavePlayerRoundStats(player);
                RemoveAsSpectator(player);
            }
            CurrentGameMode?.RemovePlayer(player);
            base.RemovePlayer(player);
            RoundCheckForEnoughAlive();
        }

        private void SetPlayerReadyForRound(TDSPlayer character)
        {
            Client player = character.Client;
            if (character.Team != null && !character.Team.IsSpectator)
            {
                Position4DDto? spawndata = GetMapRandomSpawnData(character.Team);
                if (spawndata == null)
                    return;
                NAPI.Player.SpawnPlayer(player, spawndata.ToVector3(), spawndata.Rotation);
                if (character.Team.SpectateablePlayers != null && !character.Team.SpectateablePlayers.Contains(character))
                    character.Team.SpectateablePlayers?.Add(character);
            }
            else
                NAPI.Player.SpawnPlayer(player, SpawnPoint, LobbyEntity.DefaultSpawnRotation);

            RemoveAsSpectator(character);

            Workaround.FreezePlayer(player, true);
            GivePlayerWeapons(player);

            if (_removeSpectatorsTimer.ContainsKey(character))
                _removeSpectatorsTimer.Remove(character);

            character.CurrentRoundStats?.Clear();
        }

        private void RemovePlayerFromAlive(TDSPlayer player)
        {
            if (player.Team != null)
            {
                player.Team.AlivePlayers?.Remove(player);
                --player.Team.SyncedTeamData.AmountPlayers.AmountAlive;
            }

            CurrentGameMode?.RemovePlayerFromAlive(player);

            _removeSpectatorsTimer[player] = new TDSTimer(() =>
            {
                PlayerCantBeSpectatedAnymore(player);
                SpectateOtherSameTeam(player);
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

        private void AddPlayerAsPlayer(TDSPlayer character, int teamIndex)
        {
            character.Team = LobbyEntity.LobbyRoundSettings.MixTeamsAfterRound ? GetTeamWithFewestPlayer() : Teams[teamIndex];
            character.Team.SyncAddedPlayer(character);

            if (CurrentRoundStatus == ERoundStatus.Countdown)
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
                    if (CurrentRoundStatus != ERoundStatus.None)
                        SetRoundStatus(ERoundStatus.RoundEnd, ERoundEndReason.NewPlayer);
                    else
                        SetRoundStatus(ERoundStatus.NewMapChoose);
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
            CurrentGameMode?.SendPlayerRoundInfoOnJoin(player);

            switch (CurrentRoundStatus)
            {
                case ERoundStatus.Countdown:
                    NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.CountdownStart, _nextRoundStatusTimer?.RemainingMsToExecute ?? 0);
                    break;

                case ERoundStatus.Round:
                    NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.RoundStart, true, _nextRoundStatusTimer?.RemainingMsToExecute ?? 0);
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

        private static void SavePlayerRoundStats(TDSPlayer character)
        {
            if (character.CurrentLobbyStats == null)
                return;

            PlayerLobbyStats? to = character.CurrentLobbyStats;
            RoundStatsDto? from = character.CurrentRoundStats;
            if (to == null || from == null)
                return;
            to.Kills += from.Kills;
            to.Assists += from.Assists;
            to.Damage += from.Damage;
            to.TotalKills += from.Kills;
            to.TotalAssists += from.Assists;
            to.TotalDamage += from.Damage;

            ++to.TotalRounds;
            if (from.Kills > to.MostKillsInARound)
                to.MostKillsInARound = from.Kills;
            if (from.Damage > to.MostDamageInARound)
                to.MostDamageInARound = from.Damage;
            if (from.Assists > to.MostAssistsInARound)
                to.MostAssistsInARound = from.Assists;

            from.Clear();
        }

        private void RespawnPlayer(TDSPlayer player)
        {
            SetPlayerReadyForRound(player);
            Workaround.FreezePlayer(player.Client, false);
            player.Client.TriggerEvent(DToClientEvent.PlayerRespawned);
        }
    }
}