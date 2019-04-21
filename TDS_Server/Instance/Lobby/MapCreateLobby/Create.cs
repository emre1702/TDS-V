using System.IO;
using System.Xml;
using System.Xml.Serialization;
using TDS_Common.Dto.Map;

namespace TDS_Server.Instance.Lobby
{
    partial class MapCreateLobby
    {

        public bool Create(string mapXml)
        {
            var serializer = new XmlSerializer(typeof(MapFileDto));
            using var stringReader = new StringReader(mapXml);
            using var xmlReader = XmlReader.Create(stringReader);
            if (!serializer.CanDeserialize(xmlReader))
                return false;
            serializer.Deserialize(xmlReader);

            return true;
        }
    }
}
