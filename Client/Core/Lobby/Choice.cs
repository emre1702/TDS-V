using TDS_Client.Manager.Utility;
using TDS_Shared.Default;

namespace TDS_Client.Manager.Lobby
{
    internal static class Choice
    {

        public static void JoinArena()
        {
            EventsSender.Send(ToServerEvent.JoinLobby, Settings.ArenaLobbyId);
        }

        public static void JoinMapCreator()
        {
            EventsSender.Send(ToServerEvent.JoinLobby, Settings.MapCreatorLobbyId);
        }
    }
}
