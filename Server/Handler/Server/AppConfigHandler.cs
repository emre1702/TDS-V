using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using TDS.Server.Data.Models.AppConfig;

namespace TDS.Server.Handler.Server
{
    public class AppConfigHandler
    {
        public string ConnectionString => _localSettings.ConnectionString.Value;
        public string GitHubToken => _localSettings.GitHubToken.Value;
        public List<AppConfigLoggingSetting> Logging => _localSettings.Logging;

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
                string? path = Assembly.GetExecutingAssembly().Location;
                return Path.GetDirectoryName(path ?? ".") ?? ".";
            }
        }
    }
}