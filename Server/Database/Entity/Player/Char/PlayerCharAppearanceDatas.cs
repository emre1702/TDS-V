using Newtonsoft.Json;
using TDS_Shared.Data.Models.CharCreator;

namespace TDS_Server.Database.Entity.Player.Char
{
    public class PlayerCharAppearanceDatas : CharCreateAppearanceData
    {
        #region Public Properties

        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonIgnore]
        public virtual PlayerCharDatas CharDatas { get; set; }

       

        #endregion Public Properties
    }
}
