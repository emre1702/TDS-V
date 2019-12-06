using MessagePack;

namespace TDS_Server.Dto
{
    [MessagePackObject]
    public class InvitationDto
    {
        [Key(0)]
        public ulong Id { get; set; }

        [Key(1)]
        public string Sender { get; set; } = string.Empty;

        [Key(2)]
        public string Message { get; set; } = string.Empty;
    }
}
