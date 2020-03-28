using System.IO;
using System.Xml;
using System.Xml.Serialization;
using TDS_Server.Data.Models;

namespace TDS_Server.Handler.Server
{
    public class AppConfigHandler
    {
        public string ConnectionString => _localSettings.ConnectionString.Value; 

        private readonly AppConfigDto _localSettings;

        public AppConfigHandler()
        {
            string path = "C:\\RAGEMP\\server-files\\TDS_Server.config";
            using var fileStream = new FileStream(path, FileMode.Open);
            using var reader = XmlReader.Create(fileStream);
            var xmlSerializer = new XmlSerializer(typeof(AppConfigDto));

            _localSettings = (AppConfigDto)xmlSerializer.Deserialize(reader);

            Z.EntityFramework.Extensions.LicenseManager.AddLicense(_localSettings.EFExtensions.Name, _localSettings.EFExtensions.Key);

            if (!Z.EntityFramework.Extensions.LicenseManager.ValidateLicense(out string licenseErrorMessage))
            {
                System.Console.WriteLine(licenseErrorMessage);
            }
        }
    }
}
