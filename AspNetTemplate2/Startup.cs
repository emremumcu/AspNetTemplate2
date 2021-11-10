namespace AspNetTemplate2
{
    using AspNetTemplate2.AppLib.Ext.Startup;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        /// <summary>
        /// When the application is requested for the first time, it calls ConfigureServices method. 
        /// ASP.net core has built-in support for Dependency Injection. We can add services to DI container using this method.
        /// Use ConfigureServices method to configure Dependency Injection (services).
        /// Services are typically resolved from DI using constructor injection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public void ConfigureServices(IServiceCollection services)
        {
            services._InitMVC();            

            services._AddSession();

            services._AddOptions();

            services._AddAuthentication();

            services._AddAuthorization();
        }

        /// <summary>
        /// This method is used to define how the application will respond on each HTTP request.
        /// This method is also used to configure middleware in HTTP pipeline.
        /// Use Configure method to set up middlewares, routing etc.
        /// The request handling pipeline is composed as a series of middleware components. 
        /// Each component performs operations on an HttpContext and either invokes the next middleware in the pipeline or terminates the request.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app._InitApp(env);

            app._UseSCP();

            app.UseStaticFiles();

            app._InitOptions();

            app.UseRouting();

            app._UseSession();

            app._UseAuthentication();

            app._UseAuthorization();            

            app._UseEndpoints();

            app.Run(async (context) => {
                await context.Response.WriteAsync("Service was unable to handle this request.");
            });
        }
    }
}

// ------------------------------------------------------------------------------------------------------------
// The following Startup.Configure method adds security-related middleware components in the recommended order:
// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-6.0
// ------------------------------------------------------------------------------------------------------------

//public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//{
//    if (env.IsDevelopment()) {
//        app.UseDeveloperExceptionPage();
//        app.UseDatabaseErrorPage();
//    }
//    else {
//        app.UseExceptionHandler("/Error");
//        app.UseHsts();
//    }
//    app.UseHttpsRedirection();
//    app.UseStaticFiles();
//    app.UseCookiePolicy();
//    app.UseRouting();
//    app.UseRequestLocalization();
//    app.UseCors();
//    app.UseAuthentication();
//    app.UseAuthorization();
//    app.UseSession();
//    app.UseResponseCaching();
//    app.UseEndpoints(endpoints =>
//    {
//        endpoints.MapRazorPages();
//        endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
//    });
//}