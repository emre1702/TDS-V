using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace TDS_Server.Manager.Helper
{
    internal static class XmlHelper
    {
        public static async Task<string> GetPrettyAsync(string xml)
        {
            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            using var xmlWriter = XmlWriter.Create(stringBuilder, settings);
            await element.SaveAsync(xmlWriter, CancellationToken.None);

            return stringBuilder.ToString();
        }
    }
}