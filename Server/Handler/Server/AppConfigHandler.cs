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
        #region Private Fields

        private readonly AppConfigDto _localSettings;

        #endregion Private Fields

        #region Public Constructors

        public AppConfigHandler()
        {
            string path = Path.Join(AssemblyDirectory, "TDS_Server.config");
            using var fileStream = new FileStream(path, FileMode.Open);
            using var reader = XmlReader.Create(fileStream);
            var xmlSerializer = new XmlSerializer(typeof(AppConfigDto));

            _localSettings = (AppConfigDto)xmlSerializer.Deserialize(reader);
        }

        #endregion Public Constructors

        #region Public Properties

        public string ConnectionString => _localSettings.ConnectionString.Value;

        #endregion Public Properties

        #region Private Properties

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

        #endregion Private Properties
    }
}
