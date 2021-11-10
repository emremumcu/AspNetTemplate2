namespace AspNetTemplate2.AppLib.Ext.Startup
{
    using AspNetTemplate2.AppLib.Options;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class Options
    {
        public static IServiceCollection _AddOptions(this IServiceCollection services)
        {
            services.AddOptions();

            IConfigurationBuilder builder = new ConfigurationBuilder()
               .AddJsonFile("data.json", optional: true, reloadOnChange: true)
            ;

            IConfigurationRoot ConfigurationRoot = builder.Build();

            services.Configure<DataOptions>(ConfigurationRoot.GetSection("OptGroup1"));

            return services;
        }

        public static IApplicationBuilder _InitOptions(this IApplicationBuilder app)
        {
            return app;
        }
    }
}