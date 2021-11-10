namespace AspNetTemplate2.AppLib.Ext
{
    using AspNetTemplate2.AppLib.Concrete;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    public static class ClaimsIdentityExt
    {
        // public static string GetClaim<T>(this ClaimsIdentity ci) where T: class, new() { }

        public static bool HasRole(this ClaimsIdentity ci, string roleName)
        {
            return ci.GetRoles().Any(r => r.Contains(roleName));
        }

        public static string GetNameIdentifier(this ClaimsIdentity ci)
        {
            return ci.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
        }

        public static string GetSid(this ClaimsIdentity ci)
        {
            return ci.Claims.Where(c => c.Type == ClaimTypes.Sid).FirstOrDefault().Value;
        }

        public static string GetNameSurname(this ClaimsIdentity ci)
        {
            return $"{ci.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value} {ci.Claims.Where(c => c.Type == ClaimTypes.Surname).FirstOrDefault().Value}";
        }

        public static List<string> GetRoles(this ClaimsIdentity ci)
        {
            return ci.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value.Split(Constants.Claims_Role_Seperator).ToList();
        }

        public static string GetEmail(this ClaimsIdentity ci)
        {
            return ci.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault().Value;
        }

        public static string GetUnitekodu(this ClaimsIdentity ci)
        {
            return ci.Claims.Where(c => c.Type == "KullaniciUniteKod").FirstOrDefault().Value;
        }

        public static string GetUnvan(this ClaimsIdentity ci)
        {
            return ci.Claims.Where(c => c.Type == "Unvan").FirstOrDefault().Value;
        }

        public static string GetLastTimestamp(this ClaimsIdentity ci)
        {
            return $"{ci.Claims.Where(c => c.Type == "UygulamaSonTarih").FirstOrDefault().Value} {ci.Claims.Where(c => c.Type == "UygulamaSonZaman").FirstOrDefault().Value}";
        }
    }
}
