using Microsoft.Extensions.Caching.Memory;
using Rocket.Libraries.ServiceProvider.Services.Bridges;

namespace Rocket.Apps.KeyValue.DependancyInjectionBridges
{
    public class InjectorBridge : IInjectorBridge
    {
        public InjectorBridge(IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache;
        }

        public IMemoryCache MemoryCache { get; }
    }
}