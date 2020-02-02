using RAGE.Elements;
using System.Collections.Generic;
using TDS_Client.Manager.Browser;

namespace TDS_Client.Manager.Lobby
{
    internal static class Players
    {
        public static List<Player> PlayersSameLobby = new List<Player>();

        public static void Load(List<Player> players)
        {
            PlayersSameLobby = players;
            Browser.Angular.Main.LoadNamesForChat(players);
        }

        public static void Load(Player player)
        {
            PlayersSameLobby.Add(player);
            Browser.Angular.Main.AddNameForChat(player.Name);
        }

        public static void Remove(Player player, string name)
        {
            PlayersSameLobby.Remove(player);
            Browser.Angular.Main.RemoveNameForChat(name);
        }
    }
}
