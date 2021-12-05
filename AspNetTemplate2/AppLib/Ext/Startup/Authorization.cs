namespace AspNetTemplate2.AppLib.Ext.Startup
{
    using AspNetTemplate2.AppLib.Abstract;
    using AspNetTemplate2.AppLib.Concrete;
    using AspNetTemplate2.AppLib.Policies;
    using AspNetTemplate2.AppLib.Requirements;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class Authorization
    {
        public static IServiceCollection _AddAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                /*
                 * Gets or sets the default authorization policy with no policy name specified.
                 * Defaults to require authenticated users.
                 * 
                 * The DefaultPolicy is the policy that is applied when:
                 *      (*) You specify that authorization is required, either using RequireAuthorization(), by applying an AuthorizeFilter, 
                 *          or by using the[Authorize] attribute on your actions / Razor Pages.
                 *      (*) You don't specify which policy to use.
                 *      
                 */

                //options.DefaultPolicy = new AuthorizationPolicyBuilder()
                //     .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
                //     .RequireAuthenticatedUser()
                //     .AddRequirements(new BaseRequirement())
                //     .Build();

                options.DefaultPolicy = AuthorizationPolicyLibrary.defaultPolicy;

                /*
                 * Gets or sets the fallback authorization policy when no IAuthorizeData have been provided.
                 * By default the fallback policy is null.
                 * 
                 * The FallbackPolicy is applied when the following is true:
                 *      (*) The endpoint does not have any authorisation applied. No[Authorize] attribute, no RequireAuthorization, nothing.
                 *      (*) The endpoint does not have an[AllowAnonymous] applied, either explicitly or using conventions.
                 *      
                 * So the FallbackPolicy only applies if you don't apply any other sort of authorization policy, 
                 * including the DefaultPolicy, When that's true, the FallbackPolicy is used.
                 * By default, the FallbackPolicy is a no - op; it allows all requests without authorization.
                 * 
                 */

                //options.FallbackPolicy = new AuthorizationPolicyBuilder()
                //     .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
                //     .RequireAuthenticatedUser()
                //     .AddRequirements(new BaseRequirement())
                //     .Build();

                options.FallbackPolicy = AuthorizationPolicyLibrary.fallbackPolicy;

                options.AddPolicy(AdminHandler.PolicyName, AuthorizationPolicyLibrary.adminPolicy);
                options.AddPolicy(DeveloperRequirement.PolicyName, AuthorizationPolicyLibrary.developerPolicy);

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
