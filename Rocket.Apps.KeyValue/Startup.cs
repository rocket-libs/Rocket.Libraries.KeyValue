using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Apps.KeyValue.DependancyInjectionBridges;
using Rocket.Libraries.ConsulHelper.Configuration;
using Rocket.Libraries.ConsulHelper.Models;
using Rocket.Libraries.ConsulHelper.Services.ConsulRegistryReading;
using Rocket.Libraries.ConsulHelper.Services.ConsulRegistryWriting;
using Rocket.Libraries.ServiceProviders.Extensions;

namespace Rocket.Apps.KeyValue
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMemoryCache();
            services.AddHttpClient();
            services.UseRocketServiceProvider(typeof(InjectorBridge));
            ConfigureConsul(services);
        }

        // This method gets called by the rime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void ConfigureConsul(IServiceCollection services)
        {
            services.Configure<ConsulRegistrationSettings>(config =>
            {
                new ConsulSettingsInjector("Rocket.Apps.KeyValue")
                    .Inject(config);
            });
            services.AddSingleton<IConsulRegistryReader, ConsulRegistryReader>();
            services.AddHostedService<ConsulRegistryWriter>();
        }
    }
}