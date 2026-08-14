using MaintenanceSystem.Data;
using MaintenanceSystem.Helpers;
using MaintenanceSystem.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
namespace MaintenanceSystem.Controllers
{
    public class UsersController : Controller
    {
        private  ApplicationDbContext context;
        public UsersController(ApplicationDbContext _context)
        {
            context = _context;
        }
        public IActionResult Index()
        {
            if (!HttpContext.Session.IsLoggedIn())
            {
                return RedirectToAction("GoToLoginForm", "Account");
            }

            if (!HttpContext.Session.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var users = context.Users.Include(u => u.Department).ToList();

            return View(users);
        }

        public IActionResult Create()
        {
            if (!HttpContext.Session.IsLoggedIn()) 
            {
                return RedirectToAction("GoToLoginForm", "Account"); 
            }
            if (!HttpContext.Session.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Account");
            }
           
            ViewBag.Departments = context.Departments.ToList();
            return View();

        }

        public IActionResult CreateBtn(ApplicationUser user, string password)
        {
            if (!HttpContext.Session.IsLoggedIn())
            {
                return RedirectToAction("GoToLoginForm", "Account");
            }

            if (!HttpContext.Session.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            user.Password = PasswordHasher.Hash(password);
            user.CreatedAt = DateTime.Now;
            user.IsActive = true;

            context.Users.Add(user);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            if (!HttpContext.Session.IsLoggedIn())
            {
                return RedirectToAction("GoToLoginForm", "Account");
            }
            if (!HttpContext.Session.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var user = context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            { 
                return RedirectToAction("Index");
            }
            ViewBag.Departments = context.Departments.ToList();
            
            return View(user);
        }

        public IActionResult EditBtn(ApplicationUser user)
        {
            if (!HttpContext.Session.IsLoggedIn())
            {
                return RedirectToAction("GoToLoginForm", "Account"); 
            }
            if (!HttpContext.Session.IsAdmin()) 
            { 
                return RedirectToAction("AccessDenied", "Account"); 
            }
            var oldUser = context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (oldUser == null) 
            {
                return RedirectToAction("Index"); 
            }
            oldUser.FullName = user.FullName;
            oldUser.Email = user.Email;
            oldUser.Role = user.Role; 
            oldUser.DepartmentId = user.DepartmentId;
            oldUser.IsActive = user.IsActive;
            if (!string.IsNullOrEmpty(user.Password))
            {
                oldUser.Password = PasswordHasher.Hash(user.Password);
            }

            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        { 
            if (!HttpContext.Session.IsLoggedIn())
            { 
                return RedirectToAction("GoToLoginForm", "Account");
            } 
            if (!HttpContext.Session.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Account");
            }
            var user = context.Users.Include(u => u.Department).FirstOrDefault(u => u.Id == id);
            if (user == null) 
            { 
                return RedirectToAction("Index"); 
            } 
            return View(user); 
        }

        public IActionResult ToggleStatus(int id)
        { 
            if (!HttpContext.Session.IsLoggedIn()) 
            { 
                return RedirectToAction("GoToLoginForm", "Account"); 
            } 
            if (!HttpContext.Session.IsAdmin()) 
            { 
                return RedirectToAction("AccessDenied", "Account"); 
            }
            var user = context.Users.FirstOrDefault(u => u.Id == id); 
            if (user == null)
            {
                return RedirectToAction("Index"); 
            }
            user.IsActive = !user.IsActive;
            context.SaveChanges();
            return RedirectToAction("Index"); 
        }
    }
}
