using System;
using Microsoft.Extensions.Caching.Memory;
using Rocket.Services.KeyValue.Features.Caching;
using Rocket.Services.KeyValue.Models;

namespace Rocket.Services.KeyValue.Features.Repository
{
    public interface IRepositoryWriter
    {
        KeyValueContainer Delete(string key);
        KeyValueContainer Insert(KeyValueContainer container);
    }

    public class RepositoryWriter : IRepositoryWriter
    {
        private readonly ICacheSettings cacheSettings;
        private readonly IMemoryCache memoryCache;
        private readonly IRepositoryReader repositoryReader;

        public RepositoryWriter(
            ICacheSettings cacheSettings,
            IMemoryCache memoryCache,
            IRepositoryReader repositoryReader)
        {
            this.cacheSettings = cacheSettings;
            this.memoryCache = memoryCache;
            this.repositoryReader = repositoryReader;
        }

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

        public KeyValueContainer Delete(string key)
        {
            var target = repositoryReader.Get(key);
            var targetExists = target != null;
            if (targetExists)
            {
                memoryCache.Remove(key);
            }
            return target;
        }

        private void Write(KeyValueContainer container)
        {
            var lifetime = cacheSettings.GetLifetime(container);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(lifetime);
            memoryCache.Set(container.Key, container, cacheEntryOptions);
        }
    }
}