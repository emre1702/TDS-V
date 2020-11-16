using System.Xml.Serialization;

namespace TDS.Server.Data.Enums
{
    public enum MapType
    {
        [XmlEnum("normal")]
        Normal = 0,

        [XmlEnum("bomb")]
        Bomb = 1,

        [XmlEnum("sniper")]
        Sniper = 2,

        [XmlEnum("gangwar")]
        Gangwar = 3,

        [XmlEnum("armsrace")]
        ArmsRace = 4
    }
}
