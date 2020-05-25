using System.Xml.Serialization;

namespace TDS_Server.Data.Models.WeaponsMeta
{
    public class WeaponDataValue
    {
#nullable disable

        #region Public Properties

        [XmlAttribute("value")]
        public float Value { get; set; }

        #endregion Public Properties
    }
}
