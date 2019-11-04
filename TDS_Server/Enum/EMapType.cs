using System.Xml.Serialization;

namespace TDS_Server.Enum
{
    public enum EMapType
    {
        [XmlEnum("normal")]
        Normal = 0,

        [XmlEnum("bomb")]
        Bomb = 1,

        [XmlEnum("sniper")]
        Sniper = 2,

        [XmlEnum("gangwar")]
        Gangwar = 3
    }
}