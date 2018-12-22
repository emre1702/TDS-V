using GTANetworkAPI;
using TDS.server.extend;
using TDS.server.instance.player;
using TDS.server.manager.utility;

namespace TDS.server.instance.lobby.ganglobby {

    class Events : Script {

        [RemoteEvent("inviteMemberToGang")]
        public static void InviteMember ( Client player, string targetname ) {
            Character character = player.GetChar ();
            character.Gang.InviteMember ( character, targetname );
        }

        [RemoteEvent("acceptGangInvitation")]
        public static void AcceptInvitation ( Client player, uint ganguid ) {
            Character character = player.GetChar ();
            Gang gang = Gang.GetGangByUID ( ganguid );

            if ( gang != null ) {
                gang.AcceptInvitation ( character );
            } else
                player.SendLangNotification ( "gang_doesnt_exist_anymore" );
        }
    }
}