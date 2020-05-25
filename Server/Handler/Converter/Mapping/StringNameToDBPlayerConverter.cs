using AutoMapper;
using System.Threading.Tasks;
using TDS_Server.Core.Manager.PlayerManager;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Handler.Converter.Mapping
{
    internal class StringNameToDBPlayerConverter : ITypeConverter<string, Task<Players?>>
    {
        #region Private Fields

        private readonly DatabasePlayerHelper _databasePlayerHelper;

        #endregion Private Fields

        #region Public Constructors

        public StringNameToDBPlayerConverter(DatabasePlayerHelper databasePlayerHelper)
            => _databasePlayerHelper = databasePlayerHelper;

        #endregion Public Constructors

        #region Public Methods

        public async Task<Players?> Convert(string name, Task<Players?> destination, ResolutionContext _)
        {
            if (name.Length >= 2 && name[0] == '@' && name[^1] == ':')
                name = name[1..^1];

            return await _databasePlayerHelper.GetPlayerByName(name);
        }

        #endregion Public Methods
    }
}
