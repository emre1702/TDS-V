using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using TDS_Server.Data.Models;

namespace TDS_Server.Handler.Server
{
    public class AppConfigHandler
    {
        private readonly AppConfigDto _localSettings;

        public AppConfigHandler()
        {
            string path = Path.Join(AssemblyDirectory, "TDS_Server.config");
            using var fileStream = new FileStream(path, FileMode.Open);
            using var reader = XmlReader.Create(fileStream);
            var xmlSerializer = XmlSerializer.FromTypes(new[] { typeof(AppConfigDto) })[0];

            _localSettings = (AppConfigDto)xmlSerializer.Deserialize(reader);
        }

        public string ConnectionString => _localSettings.ConnectionString.Value;

        private string AssemblyDirectory
        {
            get
            {
                string? codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase ?? "TDS_Server.RAGEAPI.dll");
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path) ?? ".";
            }
        }
    }
}
