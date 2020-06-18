using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Services.KeyValue.Features.Caching;
using Rocket.Services.KeyValue.Features.PersistantValues;
using Rocket.Services.KeyValue.Features.Repository;

namespace Rocket.Services.KeyValue.Configuration
{
    public static class CustomServiceConfiguration
    {        public static void SetupCustomServices (this IServiceCollection services)
        {
            services
                .AddHostedService<PersistantValuesLoader>()
                .AddSingleton<IMemoryCache, MemoryCache>()
                .AddTransient<IRepositoryWriter, RepositoryWriter>()
                .AddTransient<IRepositoryReader, RepositoryReader>()
                .AddTransient<ICacheSettings, CacheSettings>();
        }
    }
}