using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using TDSCPServer.Controllers;

namespace TDSCPServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://*:4201")
                .UseIISIntegration()
                .Build();
    }
}
