using GTANetworkAPI;
using System.Collections.Generic;
using TDS.Default;
using TDS.Instance.Player;
using TDS.Manager.Player;
using TDS.Manager.Utility;

namespace TDS.Instance.Lobby
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
                NAPI.Chat.SendChatMessageToPlayer(character.Player, Utils.GetReplaced(character.Language.ROUND_REWARD_INFO,
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
                //SetPlayerReadyForRound(character);
                AliveOrNotDisappearedPlayers[team.Index].Add(character);
                NAPI.ClientEvent.TriggerClientEvent(character.Player, DCustomEvent.ClientCountdownStart); 
            });
            //if (currentMap.SyncData.Type == enums.MapType.BOMB)
            //    GiveBombToRandomTerrorist();*/
        }

        /*private void SendPlayerAmountInFightInfo(Client player)
        {
            List<uint> amountinteams = new List<uint>();
            List<uint> amountaliveinteams = new List<uint>();
            for (int i = 1; i < TeamPlayers.Count; i++)
            {
                amountinteams.Add((uint)TeamPlayers[i].Count);
                amountaliveinteams.Add((uint)AlivePlayers[i].Count);
            }
            PlayerAmountInFightSync(player, amountinteams, amountaliveinteams);
        }*/
    }
}
