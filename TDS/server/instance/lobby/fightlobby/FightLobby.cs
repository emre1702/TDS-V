using GTANetworkAPI;
using TDS.server.instance.damagesys;
using TDS.server.instance.lobby.interfaces;
using TDS.server.instance.player;
using TDS.server.manager.utility;

namespace TDS.server.instance.lobby {

    public partial class FightLobby : Lobby, IFight {

        public FightLobby ( string name, int id = -1 ) : base ( name, id ) {
            DmgSys = new Damagesys ( this );
        }

        public override void Remove () {
            base.Remove ();
            DmgSys = null;
        }

        public override void AddPlayer ( Character character, bool spectator = false ) {
            base.AddPlayer ( character, spectator );

            NAPI.ClientEvent.TriggerClientEvent ( character.Player, "onClientPlayerJoinLobby", ID );
            character.Player.StopSpectating ();
            character.Player.Freeze ( false );
        }

        internal void AddPlayerDefault ( Character character, bool spectator ) {
            base.AddPlayer ( character, spectator );    
        }

        public virtual void OnPlayerDeath ( Character character, Client killer, uint weapon ) {
            if ( character.Lifes > 0 ) {
                character.Lifes--;
                DeathInfoSync ( character.Player, character.Team, killer, weapon );
            }
        }

        public void KillPlayer ( Client player, string reason ) {
            player.Kill ();
            player.SendLangNotification ( reason );
        }


    }
}
