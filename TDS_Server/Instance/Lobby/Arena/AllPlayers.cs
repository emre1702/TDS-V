using GTANetworkAPI;
using TDS_Server.Manager.Utility;
using TDS_Common.Default;
using System.Linq;
using TDS_Common.Dto;
using TDS_Common.Enum;
using Newtonsoft.Json;
using System.Text;

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

            StringBuilder strbuilder = new StringBuilder();
            FuncIterateAllPlayers((character, team) =>
            {
                if (team.IsSpectator)
                    return;
                    
                uint killreward = 0;
                uint assistreward = 0;
                uint damagereward = 0;
                
                if (LobbyEntity.MoneyPerKill.HasValue)
                    killreward = (uint)(character.CurrentRoundStats.Kills * LobbyEntity.MoneyPerKill.Value);
                if (LobbyEntity.MoneyPerAssist.HasValue)
                    assistreward = (uint)(character.CurrentRoundStats.Assists * LobbyEntity.MoneyPerAssist.Value);
                if (LobbyEntity.MoneyPerDamage.HasValue)
                    damagereward = (uint)(character.CurrentRoundStats.Damage * LobbyEntity.MoneyPerDamage.Value);

                character.GiveMoney(killreward + assistreward + damagereward);

                strbuilder.Append("#o#____________________#n#");
                strbuilder.AppendFormat (character.Language.ROUND_REWARD_INFO,
                        killreward == 0 ? "-" : killreward.ToString(),
                        assistreward == 0 ? "-" : assistreward.ToString(),
                        damagereward == 0 ? "-" : damagereward.ToString(),
                        killreward + assistreward + damagereward);
                strbuilder.Append("#n##o#____________________");

                NAPI.Chat.SendChatMessageToPlayer(character.Client, strbuilder.ToString());
                strbuilder.Clear();
            });
        }

        private void SetAllPlayersInCountdown()
        {
            FuncIterateAllPlayers((character, team) =>
            {
                if (!team.IsSpectator)
                {
                    RemoveAsSpectator(character);
                    team.SpectateablePlayers.Add(character);
                }  
                SetPlayerReadyForRound(character);
                NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.CountdownStart); 
            });
            if (currentMap.SyncedData.Type == EMapType.Bomb)
                GiveBombToRandomTerrorist();
        }

        private void StartRoundForAllPlayer()
        {
            FuncIterateAllPlayers((player, team) =>
            {
                StartRoundForPlayer(player);
            });

            SyncedTeamPlayerAmountDto[] amounts = Teams.Skip(1).Select(t => t.SyncedTeamData).Select(t => t.AmountPlayers).ToArray();
            string json = JsonConvert.SerializeObject(amounts);
            SendAllPlayerEvent(DToClientEvent.AmountInFightSync, null, json); 
        }
    }
}
