using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace TDS_Server.Manager.Helper
{
    static class XmlHelper
    {
        public static string GetPretty(string xml)
        {
            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            using var xmlWriter = XmlWriter.Create(stringBuilder, settings);
            element.Save(xmlWriter);

            return stringBuilder.ToString();
        }

        
    }
}
