using MaintenanceSystem.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (!HttpContext.Session.IsLoggedIn())
            {
                return RedirectToAction("GoToLoginForm", "Account");
            }

            return View();
        }
    }
}
