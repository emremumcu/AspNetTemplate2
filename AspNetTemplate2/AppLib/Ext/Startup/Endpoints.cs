using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetTemplate2.AppLib.Ext.Startup
{
    public static class EndpointsExtension
    {
        public static IServiceCollection _InitEndpoints(this IServiceCollection services)
        {
            return services;
        }

        public static IApplicationBuilder _UseEndpoints(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                // specific route should be created before the generic route
                endpoints.MapGet("/license", async context => { await context.Response.WriteAsync(System.IO.File.ReadAllText("license.md")); });
                endpoints.MapAreaControllerRoute(name: "admin", areaName: "admin", pattern: "admin/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(name: "areaRoute", pattern: "{area:exists}/{controller}/{action}");
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            return app;
        }
    }
}
