using TDS_Client.Manager.Utility;
using TDS_Common.Default;

namespace TDS_Client.Manager.Lobby
{
    internal static class Choice
    {

        public static void JoinArena()
        {
            EventsSender.Send(DToServerEvent.JoinLobby, Settings.ArenaLobbyId);
        }

        public static void JoinMapCreator()
        {
            EventsSender.Send(DToServerEvent.JoinLobby, Settings.MapCreatorLobbyId);
        }
    }
}
