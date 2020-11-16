using Newtonsoft.Json;
using TDS.Server.Database.Interfaces;
using TDS.Shared.Data.Models.CharCreator;

namespace TDS.Server.Database.Entity.Player.Char
{
    public class PlayerCharHeritageDatas : IPlayerDataTable
    {
        public int PlayerId { get; set; }
        public byte Slot { get; set; }

        public CharCreateHeritageData SyncedData { get; set; }

        public virtual PlayerCharDatas CharDatas { get; set; }
    }
}
