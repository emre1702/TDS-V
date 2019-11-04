using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Instance.GangTeam;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Gang;

namespace TDS_Server.Instance.Lobby
{
    partial class GangLobby
    {
        private List<GangwarAreas> _gangwarAreas = new List<GangwarAreas>();

        private async Task LoadGangwarAreas(TDSNewContext dbContext)
        {
            _gangwarAreas = await dbContext.GangwarAreas.Include(a => a.OwnerGang).AsNoTracking().ToListAsync();
        }

        private Gang? GetGangwarAreaOwner(int gangwarAreaId)
        {
            var dbGang = _gangwarAreas.Where(a => a.MapId == gangwarAreaId).Select(a => a.OwnerGang).FirstOrDefault();
            if (dbGang == null)
                return null;

            return Gang.GetFromId(dbGang.Id);
        }
    }
}
