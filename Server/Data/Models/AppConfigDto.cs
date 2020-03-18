﻿using System.Xml.Serialization;

namespace TDS_Server.Data.Models
{
#nullable disable

    [XmlRoot("root")]
    public class AppConfigDto
    {
        [XmlElement]
        public AppConfigEntryDto ConnectionString { get; set; }

        [XmlElement]
        public EFExtensionsEntryDto EFExtensions { get; set; }
    }

#nullable restore
}