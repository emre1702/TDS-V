using Newtonsoft.Json;
using TDS_Shared.Data.Models.CharCreator;

namespace TDS_Server.Database.Entity.Player.Char
{
    public class PlayerCharGeneralDatas : CharCreateGeneralData
    {
        #region Public Properties

        [JsonIgnore]
        public virtual PlayerCharDatas CharDatas { get; set; }

        [JsonIgnore]
        public int Id { get; set; }

        #endregion Public Properties
    }
}
