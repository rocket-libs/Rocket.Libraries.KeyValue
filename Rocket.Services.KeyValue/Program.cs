using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Rocket.Libraries.ConsulHelper.Convenience;

namespace Rocket.Services.KeyValue
{
    public class Program
    {
       public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (System.Exception e)
            {
                logger.Error(e, "Unhandled exception caught in the global try-catch block... This is bad");
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            PortNumberProvider.Port = 7116;
            return Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>()
                        .ConfigureLogging((hostingContext, logging) =>
                        {
                            logging.ClearProviders();
                            logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                            logging.SetMinimumLevel(LogLevel.Trace);
                            logging.AddConsole();
                            logging.AddDebug();
                            logging.AddEventSourceLogger();
                        })
                        .UseNLog()
                        .UseContentRoot(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                        .UseUrls($"http://0.0.0.0:{PortNumberProvider.Port}");
                    });
        }
    }
}