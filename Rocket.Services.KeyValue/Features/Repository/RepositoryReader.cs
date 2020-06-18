using Microsoft.Extensions.Caching.Memory;
using Rocket.Services.KeyValue.Exceptions;
using Rocket.Services.KeyValue.Features.PersistantValues;
using Rocket.Services.KeyValue.Models;

namespace Rocket.Services.KeyValue.Features.Repository
{
    public interface IRepositoryReader
    {
        KeyValueContainer Get(string key);
    }

    public class RepositoryReader : IRepositoryReader
    {
        private readonly IMemoryCache memoryCache;

        public RepositoryReader(
            IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public KeyValueContainer Get(string key)
        {
            var foundItem = memoryCache.TryGetValue(key, out KeyValueContainer container);
            if (foundItem)
            {
                return container;
            }
            else
            {
                throw new UnknownKeyException();
            }
        }
    }
}