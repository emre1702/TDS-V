using System.Xml.Serialization;

namespace TDS.Server.Data.Models.WeaponsMeta
{
    public class ItemInfos
    {
#nullable disable

        #region Public Properties

        [XmlElement("Infos")]
        public ItemInfos2 ItemInfos2 { get; set; }

        #endregion Public Properties
    }
}
