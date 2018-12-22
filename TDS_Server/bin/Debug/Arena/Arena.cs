using GTANetworkAPI;
using System.Collections.Generic;
using TDS.Entity;
using TDS.Enum;
using TDS.Instance.Lobby.Interfaces;
using TDS.Instance.Player;

namespace TDS.Instance.Lobby
{

    partial class Arena : FightLobby, IRound
    {

        public Arena(Lobbies entity) : base(entity)
        {

        }

        protected override void Remove()
        {
            base.Remove();

            roundEndTimer.Kill();
            roundStartTimer.Kill();
            countdownTimer.Kill();

            if (currentMap != null && currentMap.SyncData.Type == EMapType.Bomb)
                StopRoundBomb();
        }

        public void CheckForEnoughAlive()
        {
            int teamsinround = GetTeamAmountStillInRound();
            if (teamsinround < 2)
            {
                int winnerteam = GetTeamStillInRound();
                EndRoundEarlier(ERoundEndReason.Death, winnerteam);
            }
        }

        private void RewardAllPlayer()
        {
            foreach (KeyValuePair<Character, int> entry in DmgSys.PlayerDamage)
            {
                Character character = entry.Key;
                Client player = character.Player;
                if (player.Exists)
                {
#warning Todo (also improve it)
                    /*if (character.CurrentLobby == this)
                    {
                        List<short> reward = new List<short>();
                        if (DmgSys.PlayerKills.ContainsKey(player))
                        {
                            reward.Add((short)(Money.MoneyForDict["kill"] * DmgSys.PlayerKills[player]));
                        }
                        else
                            reward.Add(0);
                        if (DmgSys.PlayerAssists.ContainsKey(player))
                        {
                            reward.Add((short)(Money.MoneyForDict["assist"] * DmgSys.PlayerAssists[player]));
                        }
                        else
                            reward.Add(0);
                        reward.Add((short)(Money.MoneyForDict["damage"] * entry.Value));

                        short total = (short)(reward[0] + reward[1] + reward[2]);
                        character.GiveMoney(total);
                        player.SendLangNotification("round_reward", reward[0].ToString(), reward[1].ToString(), reward[2].ToString(),
                                                    total.ToString());
                    }*/
                }
            }
        }
    }
}
