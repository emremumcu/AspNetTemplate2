namespace AspNetTemplate2.AppLib.Ext.Startup
{
    using AspNetTemplate2.AppLib.Abstract;
    using AspNetTemplate2.AppLib.Concrete;
    using AspNetTemplate2.AppLib.Policies;
    using AspNetTemplate2.AppLib.Requirements;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class Authorization
    {
        public static IServiceCollection _AddAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                     .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
                     .RequireAuthenticatedUser()
                     .AddRequirements(new BaseRequirement())
                     .Build();

                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                     .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
                     .RequireAuthenticatedUser()
                     .Build();

                options.AddPolicy(AdminHandler.PolicyName, AuthorizationPolicies.adminPolicy);
                options.AddPolicy(DeveloperRequirement.PolicyName, AuthorizationPolicies.developerPolicy);

            });

            services.AddSingleton<IAuthorizationHandler, BaseHandler>();
            services.AddSingleton<IAuthorizationHandler, DeveloperHandler>();
            services.AddSingleton<IAuthorizationHandler, AdminHandler>();
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

            // TODO Set Authorizer
            services.AddSingleton<IAuthorize, TestAuthorize>();

            return services;
        }

        public static IApplicationBuilder _UseAuthorization(this IApplicationBuilder app)
        {
            app.UseAuthorization();

            return app;
        }
    }
}
