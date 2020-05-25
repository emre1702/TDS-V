using Newtonsoft.Json;

namespace TDS_Server.Data.Models.Map
{
    public class LoadMapDialogMapDto
    {
        #region Public Properties

        [JsonProperty("0")]
        public int Id { get; set; }

        [JsonProperty("1")]
        public string Name { get; set; } = string.Empty;

        #endregion Public Properties
    }
}
