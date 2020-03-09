using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace TDS_Server.Core.Manager.Helper
{
    internal static class XmlHelper
    {
        public static async Task<string> GetPrettyAsync(string xml)
        {
            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                NewLineOnAttributes = false,
                Async = true,
                Encoding = Encoding.UTF8
            };
            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                await element.SaveAsync(xmlWriter, default);
            }

            return stringBuilder.ToString();
        }
    }
}