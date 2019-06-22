using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Mapping.Converter
{
    class StringNameToDBPlayerConverter : ITypeConverter<string, Task<Players?>>
    {
        public async Task<Players?> Convert(string name, Task<Players?> destination, ResolutionContext _)
        {
            Players? ret = null;
            using (var dbcontext = new TDSNewContext())
                ret = await dbcontext.Players.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower()).ConfigureAwait(false);
            return ret;
        }
    }
}
