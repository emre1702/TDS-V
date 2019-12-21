
using MessagePack;

namespace TDS_Server.Dto
{
    [MessagePackObject]
    public class AngularAnnouncementData
    {
        [Key(0)]
        public int DaysAgo { get; set; }

        [Key(1)]
        public string Text { get; set; } = string.Empty;
    }
}
