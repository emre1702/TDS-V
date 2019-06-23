using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Manager.Mapping.Converter
{
    class StringNameToDBPlayerConverter : ITypeConverter<string, Task<Players?>>
    {
        public async Task<Players?> Convert(string name, Task<Players?> destination, ResolutionContext _)
        {
            Players? ret = await Player.Player.DbContext.Players.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower()).ConfigureAwait(false);
            return ret;
        }
    }
}
