using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestWebApp.Controllers
{
    public class IndexController : Controller
    {
        //[Authorize]
        public IActionResult Registration()
        {
            return View("Registration");
        }
    }
}
