using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TDS_Server.Data.Models.WeaponsMeta
{
    #nullable disable
    [XmlRoot("CWeaponInfoBlob")]
    public class WeaponsMetaDto
    {
        [XmlElement("Infos")]
        public ItemInfosGroups Infos { get; set; }

        [XmlIgnore]
        public List<WeaponData> Datas => Infos.ItemInfos[1].ItemInfos2.Data;
    }
}
