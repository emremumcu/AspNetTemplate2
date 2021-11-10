using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetTemplate2.AppLib.Abstract
{
    public interface IAuthorize
    {
        public AuthenticationTicket GetTicket(string userId);
    }
}
