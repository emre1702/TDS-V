using System.Xml.Serialization;

namespace TDS_Server.Enum
{
    public enum EMapType
    {
        [XmlEnum("normal")]
        Normal,

        [XmlEnum("bomb")]
        Bomb
    }
}