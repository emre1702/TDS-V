using AutoMapper;
using GTANetworkAPI;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Manager.Mapping.Converter
{
    class StringNameToClientConverter : ITypeConverter<string, Client?>
    {
        public Client? Convert(string name, Client? destination, ResolutionContext _)
        {
            return Utils.FindClient(name);
        }
    }
}
