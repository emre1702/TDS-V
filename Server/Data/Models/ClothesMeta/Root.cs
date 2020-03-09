using System.Xml.Serialization;

namespace TDS_Server.Data.Models.ClothesMeta
{
    #nullable disable
    [XmlRoot("CScriptMetadata")]
    public class Root
    {
        [XmlElement("MPOutfits")]
        public Outfits Outfits { get; set; }

        //[XmlElement("BaseElements")]
        //public BaseElements BaseElements { get; set; }

        //[XmlElement("MPApparelData")]
        //public ApparelData ApparelData { get; set; }
    }
#nullable restore
}
