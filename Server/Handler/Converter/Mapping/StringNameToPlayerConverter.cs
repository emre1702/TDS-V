using AutoMapper;
using TDS_Server.Data.Interfaces;
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
            if (name.Length >= 2 && name[0] == '@' && name[^1] == ':')
                name = name[1..^1];

            return _tdsPlayerHandler.FindTDSPlayer(name);
        }
    }
}
