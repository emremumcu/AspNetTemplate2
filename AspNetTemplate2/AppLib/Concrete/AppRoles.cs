namespace AspNetTemplate2.AppLib.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    public static class AppRoles
    {
        public static readonly List<AppRole> AppRoleList;

        public static readonly AppRole Role_Admin = new AppRole() { RoleName = "ADMIN", RoleText = "Sistem Yöneticisi" };
        public static readonly AppRole Role_Developer = new AppRole() { RoleName = "DEVELOPER", RoleText = "Uygulama Geliştirici" };
        public static readonly AppRole Role_User = new AppRole() { RoleName = "USER", RoleText = "Standart Kullanıcı" };        

        static AppRoles()
        {
            AppRoleList = new List<AppRole>() { Role_Admin, Role_Developer, Role_User };
        }

        public static AppRole ToAppRole(this string rolename)
        {
            AppRole role = AppRoleList.FirstOrDefault(r => r.RoleName == rolename);
            return role ?? default(AppRole);
        }

        public static List<AppRole> ToAppRoles(this string rolenames)
        {
            List<AppRole> roles = rolenames.Split(Constants.Claims_Role_Seperator).ToList()
                .Select(s => s.ToAppRole())
                .Where(r => r != default(AppRole))
                .ToList();
            return roles;
        }
    }

    public class AppRole
    {
        public string RoleName { get; set; }
        public string RoleText { get; set; }
    }
}
