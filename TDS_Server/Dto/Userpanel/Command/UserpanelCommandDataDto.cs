using System.Collections.Generic;
using TDS_Common.Enum;

namespace TDS_Server.Dto.Userpanel.Command
{
    #nullable disable
    class UserpanelCommandDataDto
    {
        public string Command { get; set; }
        public short? MinAdminLevel { get; set; } 
        public short? MinDonation { get; set; }
        public bool VIPCanUse { get; set; }
        public bool LobbyOwnerCanUse { get; set; }

        public List<UserpanelCommandSyntaxDto> Syntaxes { get; set; } = new List<UserpanelCommandSyntaxDto>();
        public List<string> Aliases { get; set; }
        public Dictionary<ELanguage, string> Description { get; set; }
    }
    #nullable restore
}
