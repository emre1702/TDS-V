using System.Threading.Tasks;
using AutoMapper;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Helper;

namespace TDS_Server.Handler.Converter.Mapping
{
    internal class StringNameToDBPlayerConverter : ITypeConverter<string, Task<Players?>>
    {

        private readonly DatabasePlayerHelper _databasePlayerHelper;

        public StringNameToDBPlayerConverter(DatabasePlayerHelper databasePlayerHelper)
            => _databasePlayerHelper = databasePlayerHelper;

        public async Task<Players?> Convert(string name, Task<Players?> destination, ResolutionContext _)
        {
            if (name.Length >= 2 && name[0] == '@' && name[^1] == ':')
                name = name[1..^1];

            return await _databasePlayerHelper.GetPlayerByName(name).ConfigureAwait(false);
        }

    }
}
