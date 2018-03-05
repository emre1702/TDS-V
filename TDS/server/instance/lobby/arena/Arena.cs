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

        public override void OnPlayerEnterColShape ( ColShape shape, Character character ) {
            base.OnPlayerEnterColShape ( shape, character );
            if ( lobbyBombTakeCol.ContainsKey ( this ) ) {
                if ( character.Lifes > 0 && character.Team == terroristTeamID ) {
                    TakeBomb ( character );
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
            foreach ( KeyValuePair<Character, int> entry in DmgSys.PlayerDamage ) {
                Character character = entry.Key;
                Client player = character.Player;
                if ( player.Exists ) {
                    if ( character.Lobby == this ) {
                        List<short> reward = new List<short> ();
                        if ( DmgSys.PlayerKills.ContainsKey ( player ) ) {
                            reward.Add ( (short) ( Money.MoneyForDict["kill"] * DmgSys.PlayerKills[player] ) );
                        } else
                            reward.Add ( 0 );
                        if ( DmgSys.PlayerAssists.ContainsKey ( player ) ) {
                            reward.Add ( (short) ( Money.MoneyForDict["assist"] * DmgSys.PlayerAssists[player] ) );
                        } else
                            reward.Add ( 0 );
                        reward.Add ( (short) ( Money.MoneyForDict["damage"] * entry.Value ) );

                        short total = (short) ( reward[0] + reward[1] + reward[2] );
                        character.GiveMoney ( total );
                        player.SendLangNotification ( "round_reward", reward[0].ToString (), reward[1].ToString (), reward[2].ToString (),
                                                    total.ToString () );
                    }
                }
            }
        }
    }
}
