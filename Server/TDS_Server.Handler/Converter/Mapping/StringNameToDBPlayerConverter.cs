using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Handler.Converter.Mapping
{
    class StringNameToDBPlayerConverter : ITypeConverter<string, Task<Players?>>
    {
        public async Task<Players?> Convert(string name, Task<Players?> destination, ResolutionContext _)
        {
            if (name[0] == '@')
                name = name.Substring(1);
            using var dbContext = new TDSDbContext();
            Players? ret = await dbContext.Players.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower()).ConfigureAwait(false);
            return ret;
        }
    }
}
