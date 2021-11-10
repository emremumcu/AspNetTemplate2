using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetTemplate2.AppLib.Abstract
{
    public interface IAuthenticate
    {
        public bool AuthenticateUser(string userId, string password);
    }
}
