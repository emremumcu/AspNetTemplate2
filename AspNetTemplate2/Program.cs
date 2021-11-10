namespace AspNetTemplate2
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        /// <summary>
        /// On startup, an ASP.NET Core app builds a host. The host encapsulates all of the app's resources, such as:
        /// An HTTP server implementation, Middleware components, Logging, Dependency injection(DI) services, Configuration, 
        /// There are two different hosts: .NET Generic Host and ASP.NET Core Web Host.
        /// The .NET Generic Host is recommended. The ASP.NET Core Web Host is available only for backwards compatibility.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            // IServiceScope scope = host.Services.CreateScope();
            // IServiceProvider services = scope.ServiceProvider;
            // IWebHostEnvironment environment = services.GetRequiredService<IWebHostEnvironment>();

            // IConfigurationRoot configRoot = new ConfigurationBuilder()
            //     .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            //     .AddEnvironmentVariables()
            //     .AddJsonFile("appsettings.json", optional: false)
            //     .AddCommandLine(args)
            //     .Build();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
