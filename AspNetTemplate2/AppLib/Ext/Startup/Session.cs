namespace AspNetTemplate2.AppLib.Ext.Startup
{
    using AspNetTemplate2.AppLib.Concrete;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;

    public static class Session
    {
        public static IServiceCollection _AddSession(this IServiceCollection services)
        {
            // The IDistributedCache implementation is used as a backing store for session
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.Cookie.Name = Constants.Session_Cookie_Name;
                options.IdleTimeout = Constants.Session_IdleTimeout;
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            return services;
        }

        public static IApplicationBuilder _UseSession(this IApplicationBuilder app)
        {
            app.UseSession();

            return app;
        }
    }

    public static class SessionExtensions
    {
        public static void SetKey<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetKey<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public static bool HasKey(this ISession session, string key)
        {
            return session.HasKey(key);
        }
    }
}
