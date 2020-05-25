using Newtonsoft.Json;

namespace TDS_Server.Data.Models
{
    public class AngularAnnouncementData
    {
        #region Public Properties

        [JsonProperty("0")]
        public int DaysAgo { get; set; }

        [JsonProperty("1")]
        public string Text { get; set; } = string.Empty;

        #endregion Public Properties
    }
}
