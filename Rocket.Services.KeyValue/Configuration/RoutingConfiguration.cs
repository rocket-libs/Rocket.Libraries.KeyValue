using Microsoft.AspNetCore.Builder;

namespace Rocket.Services.KeyValue.Configuration
{
    public static class RoutingConfiguration
    {
        public static void ConfigureRouting(this IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseCors("free-for-all");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}