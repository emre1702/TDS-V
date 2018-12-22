using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Server.Default;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Player;
using TDS_Server.Manager.Utility;
using TDS_Common.Default;

namespace TDS_Server.Instance.Lobby
{
    partial class Arena
    {
        private void RewardAllPlayer()
        {
            if (!LobbyEntity.IsOfficial)
                return;
            if (IsEmpty())
                return;
            if (!LobbyEntity.MoneyPerKill.HasValue && !LobbyEntity.MoneyPerAssist.HasValue && !LobbyEntity.MoneyPerDamage.HasValue)
                return;

            FuncIterateAllPlayers((character, team) =>
            {
                if (team.IsSpectatorTeam)
                    return;
                    
                uint killreward = 0;
                uint assistreward = 0;
                uint damagereward = 0;
                
                if (LobbyEntity.MoneyPerKill.HasValue)
                    killreward = character.CurrentRoundStats.Kills * LobbyEntity.MoneyPerKill.Value;
                if (LobbyEntity.MoneyPerAssist.HasValue)
                    assistreward = character.CurrentRoundStats.Assists * LobbyEntity.MoneyPerAssist.Value;
                if (LobbyEntity.MoneyPerDamage.HasValue)
                    damagereward = character.CurrentRoundStats.Damage * LobbyEntity.MoneyPerDamage.Value;

                character.GiveMoney(killreward + assistreward + damagereward);
                NAPI.Chat.SendChatMessageToPlayer(character.Client, Utils.GetReplaced(character.Language.ROUND_REWARD_INFO,
                    killreward == 0 ? "-" : killreward.ToString(),
                    assistreward == 0 ? "-" : assistreward.ToString(),
                    damagereward == 0 ? "-" : damagereward.ToString(),
                    killreward + assistreward + damagereward)
                );

            });
        }

        private void SetAllPlayersInCountdown()
        {
            FuncIterateAllPlayers((character, team) =>
            {
                if (!team.IsSpectatorTeam)
                    RemoveAsSpectator(character);
                SetPlayerReadyForRound(character);
                SpectateablePlayers[team.Index].Add(character);
                NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.CountdownStart); 
            });
            if (currentMap.SyncedData.Type == EMapType.Bomb)
                GiveBombToRandomTerrorist();
        }

        private void SendPlayerAmountInFightInfo(Client player)
        {
            List<int> amountinteams = new List<int>();
            List<int> amountaliveinteams = new List<int>();
            for (int i = 1; i < TeamPlayers.Length; i++)
            {
                amountinteams.Add(TeamPlayers[i].Count);
                amountaliveinteams.Add(AlivePlayers[i].Count);
            }
            //PlayerAmountInFightSync(player, amountinteams, amountaliveinteams);
#warning Implement with Client implementation
        }
    }
}
