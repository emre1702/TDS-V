using MessagePack;

namespace TDS_Server.Dto.Userpanel
{
    [MessagePackObject]
    public class UserpanelSettingsSpecialDataDto
    {
        [Key(0)]
        public string Username { get; set; } = string.Empty;
        
        [Key(1)]
        public string Email { get; set; } = string.Empty;
    }
}
