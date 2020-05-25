using System.Collections.Generic;
using System.Xml.Serialization;

namespace TDS_Server.Data.Models.WeaponsMeta
{
    public class ItemInfos2
    {
#nullable disable

        #region Public Properties

        [XmlElement("Item")]
        public List<WeaponData> Data { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        #endregion Public Properties
    }
}
