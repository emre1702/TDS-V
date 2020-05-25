using System.Xml.Serialization;

namespace TDS_Server.Data.Models.ClothesMeta
{
#nullable disable

    public class Item
    {
        #region Public Properties

        [XmlElement("ComponentDrawables")]
        public ComponentDrawables ComponentDrawables { get; set; }

        [XmlElement("ComponentTextures")]
        public ComponentTextures ComponentTextures { get; set; }

        [XmlElement("PropIndices")]
        public PropIndices PropIndices { get; set; }

        [XmlElement("PropTextures")]
        public PropTextures PropTextures { get; set; }

        [XmlElement("TattooHashes")]
        public TattooHashes TattooHashes { get; set; }

        #endregion Public Properties
    }

#nullable restore
}
