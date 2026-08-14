using MaintenanceSystem.Data;
using MaintenanceSystem.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.Controllers
{
    public class DashboardController : Controller
    {
        ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            if (!HttpContext.Session.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            ViewBag.UsersCount = _context.Users.Count();
            ViewBag.DevicesCount = _context.Devices.Count();
            ViewBag.TicketsCount = _context.Tickets.Count();
            ViewBag.DepartmentsCount = _context.Departments.Count();
            ViewBag.OpenTicketsCount = _context.Tickets.Count(t => t.Status == TicketStatus.Open);
            ViewBag.AssignedTicketsCount = _context.Tickets.Count(t => t.Status == TicketStatus.Assigned);
            ViewBag.InProgressTicketsCount = _context.Tickets.Count(t => t.Status == TicketStatus.InProgress);
            ViewBag.ResolvedTicketsCount = _context.Tickets.Count(t => t.Status == TicketStatus.Resolved);
            ViewBag.ClosedTicketsCount = _context.Tickets.Count(t => t.Status == TicketStatus.Closed);

            return View();
        }
    }
}
