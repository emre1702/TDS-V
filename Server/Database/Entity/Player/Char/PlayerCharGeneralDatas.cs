using TDS.Server.Database.Interfaces;
using TDS.Shared.Data.Models.CharCreator;

namespace TDS.Server.Database.Entity.Player.Char
{
    public class PlayerCharGeneralDatas : IPlayerDataTable
    {
        public int PlayerId { get; set; }
        public byte Slot { get; set; }

        public CharCreateGeneralData SyncedData { get; set; }

        public virtual PlayerCharDatas CharDatas { get; set; }
    }
}
