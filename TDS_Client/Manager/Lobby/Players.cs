using RAGE.Elements;
using System.Collections.Generic;
using TDS_Client.Manager.Browser;

namespace TDS_Client.Manager.Lobby
{
    static class Players
    {
        public static List<Player> PlayersSameLobby = new List<Player>();

        public static void Load(List<Player> players)
        {
            PlayersSameLobby = players;
            MainBrowser.LoadPlayersForChat(players);
        }

        public static void Load(Player player)
        {
            PlayersSameLobby.Add(player);
            MainBrowser.AddPlayerForChat(player);
        }

        public static void Remove(Player player)
        {
            PlayersSameLobby.Remove(player);
            MainBrowser.RemovePlayerForChat(player);
        }
    }
}
