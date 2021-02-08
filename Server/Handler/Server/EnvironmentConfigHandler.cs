using System;
using System.Collections;
using System.Collections.Generic;

namespace TDS.Server.Handler.Server
{
    public class EnvironmentConfigHandler
    {
        public string ConnectionString => Environment.GetEnvironmentVariable("TDSV_CONNECTION_STRING")!;
        public string? GitHubToken => Environment.GetEnvironmentVariable("TDSV_GITHUB_TOKEN");

        public List<(string Level, string Path)> Logging
        {
            get
            {
                var list = new List<(string Level, string Path)>();
                foreach (DictionaryEntry variable in Environment.GetEnvironmentVariables())
                {
                    var key = variable.Key.ToString();
                    if (key?.StartsWith(_loggingEnvironmentVariablePrefix) != true)
                        continue;

                    var level = key[_loggingEnvironmentVariablePrefix.Length..];
                    list.Add((level, variable.Value!.ToString()!));
                }
                return list;
            }
        }

        private const string _loggingEnvironmentVariablePrefix = "TDSV_DATABASE_LOGGING_";
    }
}