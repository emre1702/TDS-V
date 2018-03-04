using GTANetworkAPI;
using System.Collections.Generic;
using TDS.server.enums;
using TDS.server.instance.player;
using TDS.server.manager.utility;

namespace TDS.server.instance.lobby.ganglobby {

    public class GangInvitation {
        public Client Player;
        public Gang Gang;
        public string Inviter;
    }

    partial class Gang {

        private static List<GangInvitation> gangInvitations = new List<GangInvitation> ();

        public void InviteMember ( Character character, string targetname ) {
            Client player = character.Player;

            if ( !character.Gang.IsAllowedTo ( character, GangActivity.INVITE ) ) {
                player.SendLangNotification ( "not_allowed" );
                return;
            }

            Client target = Utility.FindPlayer ( targetname );
            if ( target == null || !target.Exists ) {
                player.SendLangNotification ( "player_doesnt_exist" );
                return;
            }

            SendInvitation ( player, target );
        }

        private void SendInvitation ( Client player, Client target ) {
            gangInvitations.Add ( new GangInvitation { Player = player, Gang = this, Inviter = player.Name } );

        }

        public void AcceptInvitation ( Character character ) {
            Client player = character.Player;

            GangInvitation invitation = gangInvitations.Find ( c => c.Player == player && c.Gang == character.Gang );
            if ( invitation == null ) {
                character.Player.SendLangNotification ( "invitation_was_removed" );
                return;
            }

            gangInvitations.RemoveAll ( c => c.Player == player );

            AddMember ( character );
        }


    }
}