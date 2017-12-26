using GTANetworkAPI;
using TDS.server.instance.damagesys;
using TDS.server.instance.lobby.interfaces;
using TDS.server.instance.player;
using TDS.server.manager.utility;

namespace TDS.server.instance.lobby {

    public partial class FightLobby : Lobby, IFight {

        public FightLobby ( ) { }

        public FightLobby ( string name, int id = -1 ) : base ( name, id ) {
            DmgSys = new Damagesys ( this );
        }

        public override void Remove () {
            base.Remove ();
            DmgSys = null;
        }

        public void OnPlayerWeaponSwitch ( Client player, WeaponHash oldweapon, WeaponHash newweapon ) {
           
        }

        public override void AddPlayer ( Client player, bool spectator = false ) {
            base.AddPlayer ( player, spectator );

            player.TriggerEvent ( "onClientPlayerJoinRoundlessLobby" );
            player.StopSpectating ();
            player.Freeze ( false );
        }

        internal void AddPlayerDefault ( Client player, bool spectator ) {
            base.AddPlayer ( player, spectator );    
        }

        public virtual void OnPlayerDeath ( Client player, Client killer, uint weapon, Character character ) {
            if ( character.Lifes > 0 ) {
                character.Lifes--;
                DeathInfoSync ( player, character.Team, killer, weapon );
            }
        }

        public void KillPlayer ( Client player, string reason ) {
            player.Kill ();
            player.SendLangNotification ( reason );
        }


    }
}
