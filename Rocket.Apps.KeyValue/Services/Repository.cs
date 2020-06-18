using Microsoft.Extensions.Caching.Memory;
using Rocket.Apps.KeyValue.DependancyInjectionBridges;
using Rocket.Apps.KeyValue.Exceptions;
using Rocket.Apps.KeyValue.Models;
using Rocket.Libraries.ServiceProviders.Services;
using Rocket.Libraries.ServiceProviders.Services.Instantiatable;
using System;

namespace Rocket.Apps.KeyValue.Services
{
    public class Repository : ISingletonService
    {
        private IMemoryCache _memoryCache;

        public IService Parent { get; set; }

        public IMemoryCache MemoryCache
        {
            get
            {
                if (_memoryCache == null)
                {
                    _memoryCache = RocketServiceProvider.GetBridge<InjectorBridge>().MemoryCache;
                }
                return _memoryCache;
            }
        }

        public RocketServiceProvider RocketServiceProvider { get; set; }

        public KeyValueContainer Insert(KeyValueContainer container)
        {
            var generateKeyForUser = string.IsNullOrEmpty(container.Key);
            if (generateKeyForUser)
            {
                container.Key = Guid.NewGuid().ToString();
            }

            Write(container);
            return container;
        }

        public KeyValueContainer Get(string key)
        {
            CachePersistantValuesIfRequired();
            var foundItem = MemoryCache.TryGetValue(key, out KeyValueContainer container);
            if (foundItem)
            {
                return container;
            }
            else
            {
                throw new UnknownKeyException();
            }
        }

        public KeyValueContainer Delete(string key)
        {
            var target = Get(key);
            var targetExists = target != null;
            if (targetExists)
            {
                MemoryCache.Remove(key);
            }
            return target;
        }

        private void CachePersistantValuesIfRequired()
        {
            var persistantValuesLoader = RocketServiceProvider.GetService<PersistantValuesLoader>();
            persistantValuesLoader.LoadPermanentValuesIfNeeded();
        }

        private void Write(KeyValueContainer container)
        {
            var lifetime = GetLifetime(container);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(lifetime);
            MemoryCache.Set(container.Key, container, cacheEntryOptions);
        }

        private TimeSpan GetLifetime(KeyValueContainer container)
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