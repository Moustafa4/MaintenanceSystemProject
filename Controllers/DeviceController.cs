using MaintenanceSystem.Data;
using MaintenanceSystem.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.Controllers
{
    public class DeviceController : Controller
    {
        ApplicationDbContext context = null;

        public DeviceController(ApplicationDbContext _context)
        {
            context = _context;
        }

        public IActionResult Index()
        {
            var devices = context.Devices.ToList();

            return View(devices);
        }

        public IActionResult Create()
        {
            ViewBag.Departments = context.Departments.ToList();
            ViewBag.Users = context.Users.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Device device)
        {
            context.Devices.Add(device);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var device = context.Devices.FirstOrDefault(d => d.Id == id);

            ViewBag.Departments = context.Departments.ToList();
            ViewBag.Users = context.Users.ToList();

            return View(device);
        }

        [HttpPost]
        public IActionResult Edit(Device device)
        {
            context.Devices.Update(device);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var device = context.Devices.FirstOrDefault(d => d.Id == id);

            if (device != null)
            {
                context.Devices.Remove(device);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}