namespace TDS_Server.Data.Models
{
#nullable enable

    public class SmtpClientResponse
    {
        public bool Worked => ErrorMessage is null;
        public string? ErrorMessage { get; set; }
    }
}
