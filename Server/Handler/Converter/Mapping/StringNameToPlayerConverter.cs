using AutoMapper;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Entity.Player;
using TDS_Server.Handler.Player;

namespace TDS_Server.Handler.Converter.Mapping
{
    class StringNameToPlayerConverter : ITypeConverter<string, ITDSPlayer?>
    {
        private readonly TDSPlayerHandler _tdsPlayerHandler;

        public StringNameToPlayerConverter(TDSPlayerHandler tdsPlayerHandler)
            => _tdsPlayerHandler = tdsPlayerHandler;

        public ITDSPlayer? Convert(string name, ITDSPlayer? destination, ResolutionContext _)
        {
            if (name[0] == '@')
                name = name.Substring(1);
            return _tdsPlayerHandler.FindTDSPlayer(name);
        }
    }
}
