using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rocket.Libraries.ConsulHelper.Convenience;

namespace Rocket.Apps.KeyValue
{
    public class Program
    {
        public static void Main(string[] args)
        {
            PortNumberProvider.Port = 7116;
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls($"http://0.0.0.0:{PortNumberProvider.Port}")
                .UseStartup<Startup>();
    }
}