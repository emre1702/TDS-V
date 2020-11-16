using System.Collections.Generic;
using System.Xml.Serialization;

namespace TDS.Server.Data.Models.WeaponsMeta
{
#nullable disable

    [XmlRoot("CWeaponInfoBlob")]
    public class WeaponsMetaDto
    {
        #region Public Properties

        [XmlIgnore]
        public List<WeaponData> Datas => Infos.ItemInfos[1].ItemInfos2.Data;

        [XmlElement("Infos")]
        public ItemInfosGroups Infos { get; set; }

        #endregion Public Properties
    }
}
