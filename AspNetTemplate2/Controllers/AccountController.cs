using AspNetTemplate2.AppLib.Abstract;
using AspNetTemplate2.AppLib.Concrete;
using AspNetTemplate2.AppLib.Ext.Startup;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetTemplate2.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        [NonAction]
        private async Task<IActionResult> LoginUser(string username, string password, bool remember)
        {
            if (_authenticator.AuthenticateUser(username, password))
            {
                AuthenticationTicket ticket = _authorizer.GetTicket(username);

                ValidateTicket(ticket);

                HttpContext.Session.SetKey<bool>(Constants.SessionKeyLogin, true);

                await HttpContext.SignInAsync(
                    ticket.AuthenticationScheme,
                    ticket.Principal,
                    ticket.Properties
                );

                System.Security.Claims.ClaimsIdentity ci = ((System.Security.Claims.ClaimsIdentity)User.Identity);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                throw new Exception("Kullanıcı bilgileri hatalı.");
            }
        }

        [NonAction]
        private void ValidateTicket(AuthenticationTicket ticket)
        {
            ClaimsPrincipal userPrincipal = ticket.Principal;

            String loginUser = HttpContext.Session.GetKey<string>(Constants.SessionKeyLoginUser);
            String nameIdentifier = userPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (loginUser != nameIdentifier) throw new Exception("Meyes kullanıcı tanımı eşleşme hatası");

            String roles = userPrincipal.FindFirst(ClaimTypes.Role).Value;
            List<AppRole> appRoles = roles.ToAppRoles();
            if (appRoles.Count <= 0) throw new Exception("Meyes kullanıcı rolleri bulunamadı / geçerli değil");
        }

        [NonAction]
        private async Task<IActionResult> LogoutUser()
        {
            HttpContext.Session.Remove("LOGIN");

            HttpContext.Session.Clear();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var cookie = this.Request.Cookies[Constants.Cookie_Name];

            if (cookie != null)
            {
                var options = new CookieOptions { Expires = DateTime.Now.AddDays(-1) };
                this.Response.Cookies.Append(Constants.Cookie_Name, cookie, options);
            }

            return RedirectToAction("Login");


            //HttpContext.Session.Clear();
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //return Content("Logout complete!")
        }






        public IActionResult SCP([Bind(Prefix = "id")] int? statusCode = null)
        {
            if (statusCode.HasValue)
            {
                HttpContext.Response.StatusCode = statusCode.Value;
            }

            IStatusCodeReExecuteFeature feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            StatusCodeReExecuteFeature reExecuteFeature = feature as StatusCodeReExecuteFeature;
            IExceptionHandlerFeature exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
            IExceptionHandlerPathFeature exceptionPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            IStatusCodePagesFeature statusCodePagesFeature = HttpContext.Features.Get<IStatusCodePagesFeature>();

            string OriginalURL = string.Empty;

            if (feature != null)
            {
                OriginalURL =
                    feature?.OriginalPathBase
                    + feature?.OriginalPath
                    + feature?.OriginalQueryString;
            }

            return Content($"Status Code: {statusCode}, Path: {OriginalURL}");
        }


        private readonly IAuthenticate _authenticator;
        private readonly IAuthorize _authorizer;

        public AccountController(IAuthenticate authenticator, IAuthorize authorizer)
        {
            _authenticator = authenticator;
            _authorizer = authorizer;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, UserPrincipal());

            //// Enable ajax to send authentication cookie in subsequent requests
            //this.HttpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            //this.HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "POST, GET");

            //return RedirectToAction(actionName: "Index", controllerName: "Home");


            HttpContext.Session.SetKey<string>(Constants.SessionKeyLoginUser, username);
            return await LoginUser(username, password, true);
        }


        [NonAction]
        private ClaimsPrincipal UserPrincipal()
        {
            IEnumerable<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, "UserNameIdentifier"),
                new Claim(ClaimTypes.Name, "UserName"),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return claimsPrincipal;
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            return await LogoutUser();
        }
    }
}
