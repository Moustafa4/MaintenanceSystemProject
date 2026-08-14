using MaintenanceSystem.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
<<<<<<< HEAD
            if (!HttpContext.Session.IsLoggedIn())
=======
            var userJson = HttpContext.Session.GetString("User");

<<<<<<< HEAD
            if (userJson == null)
>>>>>>> 476f5b1d4717f0fe5829c004ae2855d845b23e8b
            {
                return RedirectToAction("GoToLoginForm", "Account");
            }
=======
            //if (userJson == null)
            //{
            //    return RedirectToAction("GoToLoginForm", "Account");
            //}
>>>>>>> ec2b887730102b64ced9ba03d4348d862e825001

            return View();
        }
    }
}
