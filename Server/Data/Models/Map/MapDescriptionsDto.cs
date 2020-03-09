﻿using System.Xml.Serialization;

namespace TDS_Server.Data.Models.Map
{
    public class MapDescriptionsDto
    {
        [XmlElement("english")]
        public string? English { get; set; }

        [XmlElement("german")]
        public string? German { get; set; }
    }
}