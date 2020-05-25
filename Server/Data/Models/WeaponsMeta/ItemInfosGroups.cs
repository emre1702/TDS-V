using System.Collections.Generic;
using System.Xml.Serialization;

namespace TDS_Server.Data.Models.WeaponsMeta
{
    public class ItemInfosGroups
    {
#nullable disable

        #region Public Properties

        [XmlElement("Item")]
        public List<ItemInfos> ItemInfos { get; set; }

        #endregion Public Properties
    }
}
