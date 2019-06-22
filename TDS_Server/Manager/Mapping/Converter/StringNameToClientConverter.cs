using AutoMapper;
using GTANetworkAPI;
using System.Linq;

namespace TDS_Server.Manager.Mapping.Converter
{
    class StringNameToClientConverter : ITypeConverter<string, Client?>
    {
        public Client? Convert(string name, Client? destination, ResolutionContext _)
        {
            Client? player = NAPI.Player.GetPlayerFromName(name);
            if (player != null && player.Exists)
                return player;
            name = name.ToLower();
            return NAPI.Pools.GetAllPlayers().FirstOrDefault(c => c.Name.ToLower().StartsWith(name));
        }
    }
}
