using System;
using System.Collections.Generic;
using System.Linq;

namespace Rocket.Services.KeyValue.Configuration
{
    public class ApplicationSettings
    {
        public static bool IsDevelopment
        {
            get
            {
                var aspnetCoreEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var developmentEnvironments = new List<string> { "development", "staging" };
                return developmentEnvironments.Any(a => a.Equals(aspnetCoreEnv, StringComparison.InvariantCultureIgnoreCase));
            }
        }
    }
}