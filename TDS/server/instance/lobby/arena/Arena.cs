using GTANetworkAPI;
using System.Collections.Generic;
using TDS.server.enums;
using TDS.server.extend;
using TDS.server.instance.lobby.interfaces;
using TDS.server.instance.player;
using TDS.server.manager.utility;

namespace TDS.server.instance.lobby {

    public partial class Arena : FightLobby, IRound {  

        public Arena ( string name, int id = -1 ) : base ( name, id ) {

        }

        public override void Remove () {
            base.Remove ();

            roundEndTimer.Kill ();
            roundStartTimer.Kill ();
            countdownTimer.Kill ();

            if ( currentMap != null && currentMap.SyncData.Type == MapType.BOMB )  
                StopRoundBomb ();
        }

        public override void OnPlayerEnterColShape ( ColShape shape, Client player ) {
            base.OnPlayerEnterColShape ( shape, player );
            Character character = player.GetChar ();
            if ( lobbyBombTakeCol.ContainsKey ( this ) ) {
                if ( character.Lifes > 0 && character.Team == terroristTeamID ) {
                    TakeBomb ( player );
                }
            }
        }

        public void CheckForEnoughAlive ( ) {
            int teamsinround = GetTeamAmountStillInRound ();
            if ( teamsinround < 2 ) {
                int winnerteam = GetTeamStillInRound ();
                EndRoundEarlier ( RoundEndReason.DEATH, winnerteam );
            }
        }

        private void RewardAllPlayer ( ) {
            foreach ( KeyValuePair<Client, int> entry in DmgSys.PlayerDamage ) {
                Client player = entry.Key;
                if ( player.Exists ) {
                    Character character = player.GetChar ();
                    if ( character.Lobby == this ) {
                        List<uint> reward = new List<uint> ();
                        if ( DmgSys.PlayerKills.ContainsKey ( player ) ) {
                            reward.Add ( (uint) ( Money.MoneyForDict["kill"] * DmgSys.PlayerKills[player] ) );
                        } else
                            reward.Add ( 0 );
                        if ( DmgSys.PlayerAssists.ContainsKey ( player ) ) {
                            reward.Add ( (uint) ( Money.MoneyForDict["assist"] * DmgSys.PlayerAssists[player] ) );
                        } else
                            reward.Add ( 0 );
                        reward.Add ( (uint) ( Money.MoneyForDict["damage"] * entry.Value ) );

                        uint total = reward[0] + reward[1] + reward[2];
                        player.GiveMoney ( total, character );
                        player.SendLangNotification ( "round_reward", reward[0].ToString (), reward[1].ToString (), reward[2].ToString (),
                                                    total.ToString () );
                    }
                }
            }
        }
    }
}
