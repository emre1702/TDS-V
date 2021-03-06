﻿using System.Xml.Serialization;

namespace TDS.Server.Data.Models.WeaponsMeta
{
#nullable disable

    public class WeaponData
    {
        #region Public Properties

        [XmlElement("ClipSize")]
        public WeaponDataValue ClipSize { get; set; }

        [XmlElement("Damage")]
        public WeaponDataValue Damage { get; set; }

        [XmlElement("HeadShotDamageModifierPlayer")]
        public WeaponDataValue HeadShotDamageModifier { get; set; }

        [XmlElement("HitLimbsDamageModifier")]
        public WeaponDataValue HitLimbsDamageModifier { get; set; }

        [XmlElement("HudDamage")]
        public WeaponDataValue HudDamage { get; set; }

        [XmlElement("MaxHeadShotDistancePlayer")]
        public WeaponDataValue MaxHeadShotDistance { get; set; }

        [XmlElement("MinHeadShotDistancePlayer")]
        public WeaponDataValue MinHeadShotDistance { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("AnimReloadRate")]
        public WeaponDataValue ReloadTime { get; set; }

        [XmlElement("TimeBetweenShots")]
        public WeaponDataValue TimeBetweenShots { get; set; }

        public float TrueDamage =>
            Damage.Value > 0 ? Damage.Value : HudDamage.Value;

        [XmlElement("WeaponRange")]
        public WeaponDataValue WeaponRange { get; set; }

        #endregion Public Properties
    }
}
