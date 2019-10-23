namespace TDS_Server.Dto
{
    #nullable disable
    class CustomLobbyTeamData
    {
        public string Name { get; set; }
        public string Color { get; set; }   // HTML (rgba(...))
        public int BlipColor { get; set; }
        public int SkinHash { get; set; }
    }
    #nullable restore
}
