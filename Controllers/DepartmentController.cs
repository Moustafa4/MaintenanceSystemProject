using MaintenanceSystem.Data;
using MaintenanceSystem.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.Controllers
{
    public class DepartmentController : Controller
    {
        ApplicationDbContext context = null;

        public DepartmentController(ApplicationDbContext _context)
        {
            context = _context;
        }

        public IActionResult Index()
        {
            var departments = context.Departments.ToList();

            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            context.Departments.Add(department);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var department = context.Departments.FirstOrDefault(d => d.Id == id);

            return View(department);
        }

        [HttpPost]
        public IActionResult Edit(Department department)
        {
            context.Departments.Update(department);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var department = context.Departments.FirstOrDefault(d => d.Id == id);

            if (department != null)
            {
                context.Departments.Remove(department);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}