using Newtonsoft.Json;
using TDS_Shared.Data.Models.CharCreator;

namespace TDS_Server.Database.Entity.Player.Char
{
    public class PlayerCharFeaturesDatas : CharCreateFeaturesData
    {
        #region Public Properties

        [JsonIgnore]
        public virtual PlayerCharDatas CharDatas { get; set; }

        [JsonIgnore]
        public int PlayerId { get; set; }

        #endregion Public Properties
    }
}
