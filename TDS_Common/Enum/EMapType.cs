using System.Xml.Serialization;

namespace TDS_Common.Enum
{
    public enum EMapType
    {
        [XmlEnum("normal")]
        Normal,

        [XmlEnum("bomb")]
        Bomb
    }
}