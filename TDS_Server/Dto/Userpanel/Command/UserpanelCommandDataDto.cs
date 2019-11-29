using MessagePack;
using System.Collections.Generic;

namespace TDS_Server.Dto.Userpanel.Command
{
    [MessagePackObject]
    #nullable disable
    public class UserpanelCommandDataDto
    {
        [Key(0)]
        public string Command { get; set; }
        [Key(1)]
        public short? MinAdminLevel { get; set; }
        [Key(2)]
        public short? MinDonation { get; set; }
        [Key(3)]
        public bool VIPCanUse { get; set; }
        [Key(4)]
        public bool LobbyOwnerCanUse { get; set; }

        [Key(5)]
        public List<UserpanelCommandSyntaxDto> Syntaxes { get; set; } = new List<UserpanelCommandSyntaxDto>();
        [Key(6)]
        public List<string> Aliases { get; set; }
        [Key(7)]
        public Dictionary<int, string> Description { get; set; }
    }
    #nullable restore
}
