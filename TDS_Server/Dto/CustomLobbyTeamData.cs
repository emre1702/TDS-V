using MessagePack;

namespace TDS_Server.Dto
{
    [MessagePackObject]
    #nullable disable
    class CustomLobbyTeamData
    {
        [Key(0)]
        public string Name { get; set; }
        [Key(1)]
        public string Color { get; set; }   // HTML (rgba(...))
        [Key(2)]
        public int BlipColor { get; set; }
        [Key(3)]
        public int SkinHash { get; set; }
    }
    #nullable restore
}
