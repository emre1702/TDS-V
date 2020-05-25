using AutoMapper;
using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Player;

namespace TDS_Server.Handler.Converter.Mapping
{
    internal class StringNameToPlayerConverter : ITypeConverter<string, ITDSPlayer?>
    {
        #region Private Fields

        private readonly TDSPlayerHandler _tdsPlayerHandler;

        #endregion Private Fields

        #region Public Constructors

        public StringNameToPlayerConverter(TDSPlayerHandler tdsPlayerHandler)
            => _tdsPlayerHandler = tdsPlayerHandler;

        #endregion Public Constructors

        #region Public Methods

        public ITDSPlayer? Convert(string name, ITDSPlayer? destination, ResolutionContext _)
        {
            if (name.Length >= 2 && name[0] == '@' && name[^1] == ':')
                name = name[1..^1];

            return _tdsPlayerHandler.FindTDSPlayer(name);
        }

        #endregion Public Methods
    }
}
