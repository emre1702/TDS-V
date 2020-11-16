using Newtonsoft.Json;
using TDS.Server.Database.Interfaces;
using TDS.Shared.Data.Models.CharCreator;

namespace TDS.Server.Database.Entity.Player.Char
{
    public class PlayerCharFeaturesDatas : IPlayerDataTable
    {

        public int PlayerId { get; set; }
        public byte Slot { get; set; }

        public CharCreateFeaturesData SyncedData { get; set; }

        public virtual PlayerCharDatas CharDatas { get; set; }

    }
}
