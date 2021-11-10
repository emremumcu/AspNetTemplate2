using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetTemplate2.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult UserInfo() => View();

        [Route("/readme")]
        public IActionResult Readme() => View();
    }
}
