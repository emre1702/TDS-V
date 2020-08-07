using AutoMapper;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Handler.Player;

namespace TDS_Server.Handler.Converter.Mapping
{
    public class IPlayerToITDSPlayerConverter : ITypeConverter<IPlayer, ITDSPlayer?>
    {
        #region Private Fields

        private readonly ITDSPlayerHandler _tdsPlayerHandler;

        #endregion Private Fields

        #region Public Constructors

        public IPlayerToITDSPlayerConverter(ITDSPlayerHandler tdsPlayerHandler)
            => _tdsPlayerHandler = tdsPlayerHandler;

        #endregion Public Constructors

        #region Public Methods

        public ITDSPlayer? Convert(IPlayer source, ITDSPlayer? destination, ResolutionContext context)
        {
            return _tdsPlayerHandler.GetIfLoggedIn(source);
        }

        #endregion Public Methods
    }
}
