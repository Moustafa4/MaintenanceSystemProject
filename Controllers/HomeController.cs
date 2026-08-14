using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var userJson = HttpContext.Session.GetString("User");

<<<<<<< HEAD
            if (userJson == null)
            {
                return RedirectToAction("GoToLoginForm", "Account");
            }
=======
            //if (userJson == null)
            //{
            //    return RedirectToAction("GoToLoginForm", "Account");
            //}
>>>>>>> ec2b887730102b64ced9ba03d4348d862e825001

            ViewBag.UserData = userJson;
            return View();
        }
    }
}
