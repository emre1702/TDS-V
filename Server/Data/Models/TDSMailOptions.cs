namespace TDS_Server.Data.Models
{
    public class TDSMailOptions
    {
        public string SenderName { get; set; } = "TDS-V";
        public string SenderAddress { get; set; } = "admin@tds-v.com";
        public string ReceiverAddress { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
