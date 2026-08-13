using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var userJson = HttpContext.Session.GetString("User");

            if (userJson == null)
            {
                return RedirectToAction("GoToLoginForm", "Account");
            }

            ViewBag.UserData = userJson;
            return View();
        }
    }
}
