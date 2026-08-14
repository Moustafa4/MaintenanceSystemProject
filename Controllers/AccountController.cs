using MaintenanceSystem.Data;
using Microsoft.AspNetCore.Mvc;
using MaintenanceSystem.Helpers;

namespace MaintenanceSystem.Controllers
{
    public class AccountController : Controller
    {
        ApplicationDbContext context = null;
        public AccountController(ApplicationDbContext _context)
        {
            context = _context;
        }
        public IActionResult GoToLoginForm()
        {
            return View("Login");
        }
        public IActionResult LoginBtn(string email, string password)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null || !PasswordHasher.Verify(password, user.Password))
            {
                ViewBag.Error = "Invalid Password";
                return View("Login");
            }

            if (!user.IsActive)
            {
                ViewBag.Error = "Your account is not active";
                return View("Login");
            }

            HttpContext.Session.SetUser(
                user.Id,
                user.FullName,
                user.Role.ToString(),
                user.DepartmentId);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult LogoutBtn()
        {
            HttpContext.Session.ClearUser();

            return RedirectToAction("GoToLoginForm");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
