using System;
using AutoMapper;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Handler.Player;

namespace TDS_Server.Handler.Converter.Mapping
{
    public class IPlayerToITDSPlayerConverter : ITypeConverter<IPlayer, ITDSPlayer?>
    {
        private readonly TDSPlayerHandler _tdsPlayerHandler;

        public IPlayerToITDSPlayerConverter(TDSPlayerHandler tdsPlayerHandler) 
            => _tdsPlayerHandler = tdsPlayerHandler;

        public ITDSPlayer? Convert(IPlayer source, ITDSPlayer? destination, ResolutionContext context)
        {
            return _tdsPlayerHandler.GetIfLoggedIn(source);
        }
    }
}
