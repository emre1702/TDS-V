using AutoMapper;
using GTANetworkAPI;
using System.Linq;
using TDS_Common.Manager.Utility;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Player;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Manager.Mapping.Converter
{
    class StringNameToPlayerConverter : ITypeConverter<string, TDSPlayer?>
    {
        public TDSPlayer? Convert(string name, TDSPlayer? destination, ResolutionContext _)
        {
            if (name[0] == '@')
                name = name.Substring(1);
            return FindPlayer(name);
        }

        private static TDSPlayer? FindPlayer(string name)
        {
            if (name[0] == '@')
                name = name.Substring(1);

            name = name.ToLower();

            var player = Player.Player.LoggedInPlayers.FirstOrDefault(p => p.DisplayName.ToLower() == name || (p.Client?.Name ?? string.Empty).ToLower() == name);
            if (player is { })
                return player;

            player = Player.Player.LoggedInPlayers.FirstOrDefault(p => p.DisplayName.ToLower().StartsWith(name) || (p.Client?.Name ?? string.Empty).ToLower().StartsWith(name));
            if (player is { })
                return player;

            return null;
        }
    }
}
