using MaintenanceSystem.Data;
using MaintenanceSystem.Helpers;
using MaintenanceSystem.Models.Enums;
using Microsoft.AspNetCore.Mvc;

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

            if (user.Role == Role.Admin)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else if (user.Role == Role.Technician)
            {
                return RedirectToAction("AssignedTickets", "Tickets");
            }
            else // Employee
            {
                return RedirectToAction("MyTickets", "Tickets");
            };
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
