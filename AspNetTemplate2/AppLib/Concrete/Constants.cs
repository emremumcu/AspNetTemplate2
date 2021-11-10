namespace AspNetTemplate2.AppLib.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class Constants
    {
        // SESSION        
        public static string Session_Cookie_Name = "asp.session.cookie";
        public static TimeSpan Session_IdleTimeout = TimeSpan.FromMinutes(20);


        // COMMON

        public static string Claims_Role_Seperator = ";";

        // AUTHENTICATION

        public static string Cookie_Name = "verisorgu.authentication.cookie";
        public static string Cookie_LoginPath = "/Account/Login";
        public static string Cookie_LogoutPath = "/Account/Logout";
        public static string Cookie_AccessDeniedPath = "/Account/AccessDenied";
        public static string Cookie_ClaimsIssuer = "verisorgu.issuer";
        public static string Cookie_ReturnUrlParameter = "ReturnUrl";
        public static TimeSpan Cookie_ExpireTimeSpan = TimeSpan.FromMinutes(20);



        // SESSION KEYS

        public static string SessionKeyLogin = "LOGIN";
        public static string SessionKeyLoginUser = "LOGINUSER";
        public static string SessionKey_SelectedRole = "SELECTED_ROLE";
        public static string SessionKey_System_Message = "SYSTEM_MESSAGE";
    }
}
