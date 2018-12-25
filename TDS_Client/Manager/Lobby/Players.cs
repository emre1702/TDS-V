using RAGE.Elements;
using System.Collections.Generic;
using TDS_Client.Manager.Browser;

namespace TDS_Client.Manager.Lobby
{
    static class Players
    {
        private static List<Player> playersSameLobby = new List<Player>();

        public static void Load(List<Player> players)
        {
            playersSameLobby = players;
            MainBrowser.LoadPlayersForChat(players);
        }

        public static void Load(Player player)
        {
            playersSameLobby.Add(player);
            MainBrowser.AddPlayerForChat(player);
        }

        public static void Remove(Player player)
        {
            playersSameLobby.Remove(player);
            MainBrowser.RemovePlayerForChat(player);
        }
    }
}
