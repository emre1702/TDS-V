using MessagePack;

namespace TDS_Server.Dto.Userpanel.Command
{
    [MessagePackObject]
    #nullable disable
    public class UserpanelCommandParameterDto
    {
        [Key(0)]
        public string Name { get; set; }
        [Key(1)]
        public string Type { get; set; }
        [Key(2)]
        public object DefaultValue { get; set; }
    }
    #nullable restore
}
