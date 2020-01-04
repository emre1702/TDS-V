using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BonusBotConnector_Server
{
    public class Program
    {
        public delegate void BonusBotErrorLoggerDelegate(string info, string stackTrace, bool logToBonusBot = true);

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        internal static BonusBotErrorLoggerDelegate ErrorLogger { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public static void Main() { }

        public static void Init(BonusBotErrorLoggerDelegate errorLogger)
        {
            ErrorLogger = errorLogger;
            CreateHostBuilder().Build().RunAsync();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseUrls("http://localhost:5001");
                });
    }
}
