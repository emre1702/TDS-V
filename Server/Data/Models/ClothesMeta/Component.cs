using System.Xml.Serialization;

namespace TDS_Server.Data.Models.ClothesMeta
{
#nullable disable

    public class Component
    {
        #region Public Properties

        [XmlAttribute("value")]
        public int Value { get; set; }

        #endregion Public Properties
    }

#nullable restore
}
