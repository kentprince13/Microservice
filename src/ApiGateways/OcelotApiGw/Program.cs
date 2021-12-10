using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OcelotApiGw
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).
                ConfigureAppConfiguration((hc, config) =>
                {
                    config.AddJsonFile($"ocelot.{hc.HostingEnvironment.EnvironmentName}.json",true,true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging((bc, lb) =>
                {
                    lb.AddConfiguration(bc.Configuration.GetSection("logging"));
                    lb.AddConsole();
                    lb.AddDebug();
                });
    }
}
