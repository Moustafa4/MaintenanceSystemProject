using MaintenanceSystem.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
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

            var userData = new
            {
                Id = user.Id,
                FullName = user.FullName,
                Role = user.Role.ToString(),
                DepartmentId = user.DepartmentId
            };

            HttpContext.Session.SetString("User", JsonSerializer.Serialize(userData));

            return RedirectToAction("Index", "Home");
        }
        public IActionResult LogoutBtn()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("GoToLoginForm");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
