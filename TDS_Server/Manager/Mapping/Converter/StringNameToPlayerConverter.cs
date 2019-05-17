using AutoMapper;
using GTANetworkAPI;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Player;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Manager.Mapping.Converter
{
    class StringNameToPlayerConverter : ITypeConverter<string, TDSPlayer?>
    {
        public TDSPlayer? Convert(string name, TDSPlayer? destination, ResolutionContext _)
        {
            Client? client = Utils.FindClient(name);
            TDSPlayer? player = client?.GetChar();
            return player != null && player.LoggedIn ? player : null;
        }
    }
}
