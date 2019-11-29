using MessagePack;

namespace TDS_Server.Dto.TeamChoiceMenu
{
    [MessagePackObject]
    public class TeamChoiceMenuTeamData
    {
        [Key(0)]
        public string Name { get; set; }
        [Key(1)]
        public short Red { get; set; }
        [Key(2)]
        public short Green { get; set; }
        [Key(3)]
        public short Blue { get; set; }


        public TeamChoiceMenuTeamData(string name, short red, short green, short blue)
        {
            Name = name;
            Red = red;
            Green = green;
            Blue = blue;
        }
    }
}
