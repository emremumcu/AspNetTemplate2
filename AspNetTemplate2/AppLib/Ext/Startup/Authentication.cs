namespace AspNetTemplate2.AppLib.Ext.Startup
{
    using AspNetTemplate2.AppLib.Abstract;
    using AspNetTemplate2.AppLib.Concrete;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading.Tasks;

    public static class AuthenticationExtension
    {
        public static IServiceCollection _AddAuthentication(this IServiceCollection services)
        {
            // A simple Authentication scenario
            services
                // .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.Cookie.Name = Constants.Cookie_Name;
                    options.LoginPath = Constants.Cookie_LoginPath;
                    options.LogoutPath = Constants.Cookie_LogoutPath;
                    options.AccessDeniedPath = Constants.Cookie_AccessDeniedPath;
                    options.ClaimsIssuer = Constants.Cookie_ClaimsIssuer;
                    options.ReturnUrlParameter = Constants.Cookie_ReturnUrlParameter;
                    options.SlidingExpiration = true;
                    options.Cookie.HttpOnly = true; // false: xss vulnerability !!!
                    options.ExpireTimeSpan = Constants.Cookie_ExpireTimeSpan;
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                    options.Validate();
                    options.EventsType = typeof(CustomCookieAuthenticationEvents);
                })
            ;

            services.AddScoped<CustomCookieAuthenticationEvents>();

            // TODO Authenticator
            services.AddSingleton<IAuthenticate, TestAuthenticate>();

            return services;
        }

        public static IApplicationBuilder _UseAuthentication(this IApplicationBuilder app)
        {
            app.UseAuthentication();

            return app;
        }
    }

    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            Boolean login = context.HttpContext.Session.GetKey<bool>(Constants.SessionKeyLogin);

            if (!(context.Principal.Identity.IsAuthenticated && login))
            {
                context.RejectPrincipal();
                await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
    }
}
