using System;
using Rocket.Services.KeyValue.Models;

namespace Rocket.Services.KeyValue.Features.Caching
{
    public interface ICacheSettings
    {
        TimeSpan GetLifetime(KeyValueContainer container);
    }

    public class CacheSettings : ICacheSettings
    {
        public TimeSpan GetLifetime(KeyValueContainer container)
        {
            var lifetimeExplicitlySpecified = container.LifetimeSeconds > 0;
            long lifetimeSeconds;
            if (lifetimeExplicitlySpecified)
            {
                lifetimeSeconds = container.LifetimeSeconds;
            }
            else
            {
                var oneMinuteSeconds = 60;
                var oneHourSeconds = oneMinuteSeconds * 60;
                var fourHourSeconds = oneHourSeconds * 4;
                lifetimeSeconds = fourHourSeconds;
            }
            return TimeSpan.FromSeconds(lifetimeSeconds);
        }
    }
}