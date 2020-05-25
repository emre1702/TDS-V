using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BonusBotConnector_Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#pragma warning disable IDE0060 // Remove unused parameter

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            /*if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }*/

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<BBCommandService>();

                endpoints.MapGet("/", async context =>
                {
                        await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client.");
                    });
            });
        }
    }
}
