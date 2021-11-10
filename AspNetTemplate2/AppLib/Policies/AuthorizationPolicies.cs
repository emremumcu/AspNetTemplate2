namespace AspNetTemplate2.AppLib.Policies
{
    using AspNetTemplate2.AppLib.Requirements;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;

    public static class AuthorizationPolicies
    {
        public static AuthorizationPolicy defaultPolicy = new AuthorizationPolicyBuilder()
           .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
           .RequireAuthenticatedUser()
           .Build();

        public static AuthorizationPolicy fallbackPolicy = new AuthorizationPolicyBuilder()
           .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
           .RequireAuthenticatedUser()
           .Build();

        public static AuthorizationPolicy adminPolicy = new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .AddRequirements(new AdminRequirement(adminRoleName: AdminHandler.RoleName))
            .Build();

        public static AuthorizationPolicy developerPolicy = new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .AddRequirements(new DeveloperRequirement())
            .Build();
    }
}
