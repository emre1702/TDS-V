using TDS_Client.Manager.Utility;
using TDS_Common.Default;

namespace TDS_Client.Manager.Lobby
{
    internal static class Choice
    {

        public static void JoinLobby(int index, uint? teamindex)
        {
            EventsSender.Send(DToServerEvent.JoinLobby, index, teamindex);
            //EventsSender.SendCooldown("joinMapCreatorLobby");
        }

        public static void JoinArena()
        {
            EventsSender.Send(DToServerEvent.JoinArena);
        }

        public static void JoinMapCreator()
        {
            EventsSender.Send(DToServerEvent.JoinMapCreator);
        }
    }
}