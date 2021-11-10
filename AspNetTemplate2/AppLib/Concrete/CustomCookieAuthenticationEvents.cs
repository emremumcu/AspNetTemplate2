namespace AspNetTemplate2.AppLib.Concrete
{
    using AspNetTemplate2.AppLib.Ext.Startup;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using System;
    using System.Threading.Tasks;

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
