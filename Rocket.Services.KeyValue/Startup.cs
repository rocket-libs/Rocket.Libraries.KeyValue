using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Services.KeyValue.Configuration;
using Rocket.Libraries.ConsulHelper.Convenience;

namespace Rocket.Services.KeyValue
{
    public class Startup
    {
        public Startup (IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure (IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (ApplicationSettings.IsDevelopment)
            {
                app.UseDeveloperExceptionPage ();
            }

            app.UseSwaggerDocumenting ();
            app.ConfigureRouting ();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services)
        {
            services.AddControllers ();
            services.RegisterSwaggerService ();
            services.AddOptions ();

            services.SetupCustomServices ();
            services.AddCors (opt => opt.AddPolicy ("free-for-all", builder =>
            {
                builder.AllowAnyHeader ()
                    .AllowAnyMethod ()
                    .AllowAnyOrigin ();
            }));
            services.AddConsulHelper (Configuration);
        }
    }
}