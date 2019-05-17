using AutoMapper;
using GTANetworkAPI;
using System.Linq;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Player;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Manager.Mapping.Converter
{
    class StringNameToPlayerConverter : ITypeConverter<string, TDSPlayer?>
    {
        public TDSPlayer? Convert(string name, TDSPlayer? destination, ResolutionContext _)
        {
            Client? client = FindClient(name);
            TDSPlayer? player = client?.GetChar();
            return player != null && player.LoggedIn ? player : null;
        }

        private static Client? FindClient(string name)
        {
            Client? player = NAPI.Player.GetPlayerFromName(name);
            if (player != null && player.Exists)
                return player;
            name = name.ToLower();
            return NAPI.Pools.GetAllPlayers().FirstOrDefault(c => c.Name.ToLower().StartsWith(name));
        }
    }
}
