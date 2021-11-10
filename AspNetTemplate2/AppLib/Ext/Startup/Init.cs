using AspNetTemplate2.AppLib.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace AspNetTemplate2.AppLib.Ext.Startup
{
    public static class Init
    {
        public static IServiceCollection _InitMVC(this IServiceCollection services)
        {
            IMvcBuilder mvcBuilder =
                services.AddControllersWithViews(config =>
                {
                    config.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    config.Filters.Add(new AuthorizeFilter());
                    config.Filters.Add<GlobalExceptionFilter>();
                })
                /// Use session based TempData instead of cookie based TempData
                .AddSessionStateTempDataProvider();

            services.Configure<CookieTempDataProviderOptions>(options =>
            {
                /// GDPR
                options.Cookie.IsEssential = true;
            });

            services.AddHttpContextAccessor();

            // IServiceProvider serviceProvider = services.BuildServiceProvider();
            // IWebHostEnvironment environment = serviceProvider.GetRequiredService<IWebHostEnvironment>();

            // if (!environment.IsProduction())
            // {
                // /// Install-Package Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation   
                // mvcBuilder.AddRazorRuntimeCompilation();
            // }

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                /// Install-Package Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation   
                mvcBuilder.AddRazorRuntimeCompilation();
            }

            mvcBuilder.AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

            services.AddRazorPages();

            return services;
        }

        public static IApplicationBuilder _InitApp(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            return app;
        }
    }
}
