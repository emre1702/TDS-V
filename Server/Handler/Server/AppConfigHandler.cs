using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using TDS.Server.Data.Models;

namespace TDS.Server.Handler.Server
{
    public class AppConfigHandler
    {
        public string ConnectionString => _localSettings.ConnectionString.Value;
        public string GitHubToken => _localSettings.GitHubToken.Value;

        private readonly AppConfigDto _localSettings;

        public AppConfigHandler()
        {
            var path = Path.Join(AssemblyDirectory, "TDS.Server.config");
            using var fileStream = new FileStream(path, FileMode.Open);
            using var reader = XmlReader.Create(fileStream);
            var xmlSerializer = XmlSerializer.FromTypes(new[] { typeof(AppConfigDto) })[0];

            _localSettings = (AppConfigDto)xmlSerializer.Deserialize(reader)!;
        }

        private string AssemblyDirectory
        {
            get
            {
                string? codeBase = Assembly.GetExecutingAssembly().Location;
                var uri = new UriBuilder(codeBase ?? "TDS.Server.RAGEAPI.dll");
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path) ?? ".";
            }
        }
    }
}
