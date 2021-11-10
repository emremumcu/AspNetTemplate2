namespace AspNetTemplate2.AppLib.Concrete
{
    using AspNetTemplate2.AppLib.Abstract;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;

    public class TestAuthorize : IAuthorize
    {
        public TestAuthorize() { }

        private ClaimsPrincipal GetPrincipal(string userId)
        {
            List<Claim> UserClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, "emumcu2"),
                new Claim(ClaimTypes.Sid, ""),
                new Claim(ClaimTypes.Name, ""),
                new Claim(ClaimTypes.Surname, ""),
                new Claim(ClaimTypes.Role, string.Join(Constants.Claims_Role_Seperator, new string[] { "ADMIN", "DEVELOPER", "USER" })),
                new Claim(ClaimTypes.Email, ""),
                new Claim("Unvan", ""),
            };

            return new ClaimsPrincipal(new ClaimsIdentity(UserClaims, CookieAuthenticationDefaults.AuthenticationScheme));
        }

        private AuthenticationProperties GetProperties() => new AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
            IsPersistent = true,
            IssuedUtc = DateTimeOffset.UtcNow,
            RedirectUri = "RedirectUri"
        };

        public AuthenticationTicket GetTicket(string userId) => new AuthenticationTicket(GetPrincipal(userId), GetProperties(), CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
