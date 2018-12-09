using System.Threading.Tasks;
using TDS.Dto;
using TDS.Instance.Player;

namespace TDS.Instance.Lobby {

    partial class Arena {

        public override async Task<bool> AddPlayer(Character character, uint teamid)
        {
            if (!await AddPlayer(character, teamid))
                return false;

            character.CurrentRoundStats = new RoundStatsDto();
#warning todo: Add spectate
            return true;


            /*
            if ( !AddPlayerDefault ( character, spectator ) )
				return false;

            string mapname = currentMap != null ? currentMap.SyncData.Name : "unknown";
            NAPI.ClientEvent.TriggerClientEvent ( character.Player, "onClientPlayerJoinLobby", ID, spectator, mapname, JsonConvert.SerializeObject ( Teams ), JsonConvert.SerializeObject ( teamColorsList ), 
                                countdownTime, roundTime, bombDetonateTime, bombPlantTime, bombDefuseTime,
                                RoundEndTime, true );

            if ( !spectator )
                AddPlayerAsPlayer ( character );

            SendPlayerRoundInfoOnJoin ( character.Player );

			return true;*/
        }

        public override void RemovePlayer(Character character)
        {
            if (character.Lifes > 0)
            {
                //Damagesys.CheckLastHitter(character, out Character killercharacter);
                //DeathInfoSync(character.Player, character.Team, killercharacter?.Player, (uint)WeaponHash.Unarmed);
                //RemovePlayerFromAlive(character);
                //Damagesys.PlayerSpree.Remove(character);
            } 
            else
                RemoveAsSpectator(character);
            base.RemovePlayer(character);
        }

        /*public uint Lifes = 1;


        private void SetPlayerReadyForRound ( Character character ) {
            Client player = character.Player;
            player.Armor = (int) Armor;
            player.Health = (int) Health;
            Spectate ( character, character );
            RemoveAsSpectator(character);
            if ( character.Team > 0 ) {
                Vector3[] spawndata = GetMapRandomSpawnData ( character.Team );
                player.Position = spawndata[0];
                player.Rotation = spawndata[1];
            }
            player.Freeze ( true );
            GivePlayerWeapons ( player );
        }


        private void RemovePlayerFromAlive ( Character character ) {
            int teamID = character.Team;
            character.Lifes = 0;
            int aliveindex = AlivePlayers[teamID].IndexOf ( character );
            PlayerCantBeSpectatedAnymore ( character, aliveindex, teamID );
            AlivePlayers[teamID].RemoveAt ( aliveindex );
            if ( bombAtPlayer == character ) {
                DropBomb ();
            }
            CheckForEnoughAlive ();
        }


        private void AddPlayerAsPlayer ( Character character ) {
            int teamID = GetTeamIDWithFewestMember ( ref TeamPlayers );
            SetPlayerTeam ( character, teamID );
            if ( countdownTimer != null && countdownTimer.IsRunning ) {
                SetPlayerReadyForRound ( character );
            } else {
                int teamsinround = GetTeamAmountStillInRound ();
                NAPI.Util.ConsoleOutput ( teamsinround + " teams still in round" );
                if ( teamsinround < 2 ) {
                    EndRoundEarlier ( RoundEndReason.NEWPLAYER, character.Player.Name );
                    NAPI.Util.ConsoleOutput ( "End round earlier because of joined player" );
                } else {
                    RespawnPlayerInSpectateMode ( character );
                }
            }
        }

        public static void PlayerAmountInFightSync ( Client player, List<uint> amountinteam, List<uint> amountaliveinteam ) {
            NAPI.ClientEvent.TriggerClientEvent ( player, "onClientPlayerAmountInFightSync", JsonConvert.SerializeObject ( amountinteam ), 1, JsonConvert.SerializeObject ( amountaliveinteam ) );
        }

        private void SendPlayerRoundInfoOnJoin ( Client player ) {
            if ( currentMap != null ) {
                NAPI.ClientEvent.TriggerClientEvent ( player, "onClientMapChange", currentMap.SyncData.Name, JsonConvert.SerializeObject ( currentMap.MapLimits ), currentMap.MapCenter.X, currentMap.MapCenter.Y, currentMap.MapCenter.Z );
            }

            SendPlayerAmountInFightInfo ( player );
            SyncMapVotingOnJoin ( player );


            int tick = Environment.TickCount;
            switch ( status ) {
                case LobbyStatus.COUNTDOWN:
                    Map map = currentMap;
                    if ( map != null )
                        NAPI.ClientEvent.TriggerClientEvent ( player, "onClientCountdownStart", tick - startTick );
                    break;
                case LobbyStatus.ROUND:
                    NAPI.ClientEvent.TriggerClientEvent ( player, "onClientRoundStart", 1, tick - startTick );
                    break;
            }
        }*/

    }
}
