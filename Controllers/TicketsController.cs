using MaintenanceSystem.Data;
using MaintenanceSystem.Helpers;
using MaintenanceSystem.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext context;

        public TicketsController(ApplicationDbContext _context)
        {
            context = _context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MyTickets()
        {
            if (!HttpContext.Session.IsLoggedIn())
            {
                return RedirectToAction("GoToLoginForm", "Account");
            }

            var currentUser = HttpContext.Session.GetUser();

            var tickets = context.Tickets
                .Include(t => t.Device)
                .Include(t => t.Department)
                .Where(t => t.CreatedByUserId == currentUser.Id)
                .ToList();

            return View(tickets);
        }
        public IActionResult Create() 
        {
            if (!HttpContext.Session.IsLoggedIn())
            {
                return RedirectToAction("GoToLoginForm", "Account");
            }
            var currentUser = HttpContext.Session.GetUser();
            if (currentUser.Role != "Employee")
            { 
                return RedirectToAction("AccessDenied", "Account");
            } 
            ViewBag.Devices = context.Devices.Include(d => d.Department).ToList();
            return View(); 
        }
        public IActionResult CreateBtn(Ticket ticket)
        { 
            if (!HttpContext.Session.IsLoggedIn())
            { 
                return RedirectToAction("GoToLoginForm", "Account");
            } var currentUser = HttpContext.Session.GetUser(); 
            if (currentUser.Role != "Employee") 
            { 
                return RedirectToAction("AccessDenied", "Account"); 
            } 
            var device = context.Devices.FirstOrDefault(d => d.Id == ticket.DeviceId); 
            if (device == null) 
            { 
                return RedirectToAction("Create");
            } 
            ticket.CreatedByUserId = currentUser.Id;
            ticket.Status = TicketStatus.Open;
            ticket.CreatedAt = DateTime.Now;
            ticket.DepartmentId = device.DepartmentId;
            ticket.DeviceOwnerUserId = currentUser.Id; 
            context.Tickets.Add(ticket); 
            context.SaveChanges(); 
            return RedirectToAction("MyTickets");
        }
   
        public IActionResult Details(int id)
        {
            if (!HttpContext.Session.IsLoggedIn())
            {
                return RedirectToAction("GoToLoginForm", "Account");
            }

            var currentUser = HttpContext.Session.GetUser();

            var ticket = context.Tickets
                .Include(t => t.Device)
                .Include(t => t.Department)
                .Include(t => t.CreatedByUser)
                .Include(t => t.AssignedTechnician)
                .Include(t => t.CancelledByUser)
                .FirstOrDefault(t => t.Id == id);

            if (ticket == null)
            {
                return RedirectToAction("MyTickets");
            }

            if (currentUser.Role == "Employee" &&
                ticket.CreatedByUserId != currentUser.Id)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (currentUser.Role == "Technician" &&
                ticket.AssignedTechnicianId != currentUser.Id)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            return View(ticket);
        }
        
    public IActionResult OpenTickets()
            {
                if (!HttpContext.Session.IsLoggedIn())
                {
                    return RedirectToAction("GoToLoginForm", "Account");
                }

                if (!HttpContext.Session.IsAdmin())
                {
                    return RedirectToAction("AccessDenied", "Account");
                }

                var tickets = context.Tickets
                    .Include(t => t.Device)
                    .Include(t => t.Department)
                    .Include(t => t.CreatedByUser)
                    .Where(t => t.Status == TicketStatus.Open)
                    .ToList();

                return View(tickets);
        }
       
        public IActionResult Assign(int id)
                {
                    if (!HttpContext.Session.IsLoggedIn())
                    {
                        return RedirectToAction("GoToLoginForm", "Account");
                    }

                    if (!HttpContext.Session.IsAdmin())
                    {
                        return RedirectToAction("AccessDenied", "Account");
                    }

                    var ticket = context.Tickets
                        .Include(t => t.Device)
                        .FirstOrDefault(t => t.Id == id);

                    if (ticket == null)
                    {
                        return RedirectToAction("OpenTickets");
                    }

                    var technicians = context.Users
                        .Where(u => u.Role == MaintenanceSystem.Models.Enums.Role.Technician
                                    && u.IsActive)
                        .ToList();

                    ViewBag.Technicians = technicians;

                    return View(ticket);
        }

    public IActionResult AssignBtn(int ticketId, int technicianId)
            {
                if (!HttpContext.Session.IsLoggedIn())
                {
                    return RedirectToAction("GoToLoginForm", "Account");
                }

                if (!HttpContext.Session.IsAdmin())
                {
                    return RedirectToAction("AccessDenied", "Account");
                }

                var ticket = context.Tickets.FirstOrDefault(t => t.Id == ticketId);

                var technician = context.Users
                    .FirstOrDefault(u => u.Id == technicianId
                                      && u.Role == MaintenanceSystem.Models.Enums.Role.Technician
                                      && u.IsActive);

                if (ticket == null || technician == null)
                {
                    return RedirectToAction("OpenTickets");
                }

                ticket.AssignedTechnicianId = technician.Id;
                ticket.Status = TicketStatus.Assigned;
                ticket.AssignedAt = DateTime.Now;

                context.SaveChanges();

                return RedirectToAction("OpenTickets");
        }

        
        public IActionResult AssignedTickets()
                {
                    if (!HttpContext.Session.IsLoggedIn())
                    {
                        return RedirectToAction("GoToLoginForm", "Account");
                    }

                    if (!HttpContext.Session.IsTechnician())
                    {
                        return RedirectToAction("AccessDenied", "Account");
                    }

                    var currentUser = HttpContext.Session.GetUser();

                    var tickets = context.Tickets
                        .Include(t => t.Device)
                        .Include(t => t.Department)
                        .Include(t => t.CreatedByUser)
                        .Where(t => t.AssignedTechnicianId == currentUser.Id)
                        .ToList();

                    return View(tickets);
        }

    public IActionResult UpdateStatus(int id, TicketStatus status)
            {
                if (!HttpContext.Session.IsLoggedIn())
                {
                    return RedirectToAction("GoToLoginForm", "Account");
                }

                if (!HttpContext.Session.IsTechnician())
                {
                    return RedirectToAction("AccessDenied", "Account");
                }

                var currentUser = HttpContext.Session.GetUser();

                var ticket = context.Tickets.FirstOrDefault(t => t.Id == id);

                if (ticket == null)
                {
                    return RedirectToAction("AssignedTickets");
                }

                if (ticket.AssignedTechnicianId != currentUser.Id)
                {
                    return RedirectToAction("AccessDenied", "Account");
                }

                if (status == TicketStatus.InProgress &&
                    ticket.Status == TicketStatus.Assigned)
                {
                    ticket.Status = TicketStatus.InProgress;
                    ticket.InProgressAt = DateTime.Now;
                }
                else if (status == TicketStatus.Resolved &&
                         ticket.Status == TicketStatus.InProgress)
                {
                    ticket.Status = TicketStatus.Resolved;
                    ticket.ResolvedAt = DateTime.Now;
                }

                context.SaveChanges();

                return RedirectToAction("AssignedTickets");
        }

 
       public IActionResult CloseTicket(int id)
        {
            if (!HttpContext.Session.IsLoggedIn())
            {
                return RedirectToAction("GoToLoginForm", "Account");
            }

            var currentUser = HttpContext.Session.GetUser();

            var ticket = context.Tickets.FirstOrDefault(t => t.Id == id);

            if (ticket == null)
            {
                return RedirectToAction("MyTickets");
            }

            if (currentUser.Role != "Admin" &&
                ticket.CreatedByUserId != currentUser.Id)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (ticket.Status != TicketStatus.Resolved)
            {
                return RedirectToAction("Details", new { id = ticket.Id });
            }

            ticket.Status = TicketStatus.Closed;
            ticket.ClosedAt = DateTime.Now;

            context.SaveChanges();

            if (currentUser.Role == "Admin")
            {
                return RedirectToAction("OpenTickets");
            }

            return RedirectToAction("MyTickets");
        }
   
    public IActionResult AllTickets()
        {
            if (!HttpContext.Session.IsLoggedIn())
            {
                return RedirectToAction("GoToLoginForm", "Account");
            }

            if (!HttpContext.Session.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var tickets = context.Tickets
                .Include(t => t.Device)
                .Include(t => t.Department)
                .Include(t => t.CreatedByUser)
                .Include(t => t.AssignedTechnician)
                .Include(t => t.CancelledByUser)
                .ToList();

            return View(tickets);
        }


     public IActionResult CancelTicket(int id)
        {
            if (!HttpContext.Session.IsLoggedIn())
            {
                return RedirectToAction("GoToLoginForm", "Account");
            }

            var currentUser = HttpContext.Session.GetUser();

            var ticket = context.Tickets.FirstOrDefault(t => t.Id == id);

            if (ticket == null)
            {
                return RedirectToAction("MyTickets");
            }

            if (currentUser.Role == "Admin")
            {
                ticket.Status = TicketStatus.Cancelled;
            }
            else if (currentUser.Role == "Employee" &&
                     ticket.CreatedByUserId == currentUser.Id)
            {
                ticket.Status = TicketStatus.Cancelled;
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            ticket.CancelledByUserId = currentUser.Id;

            context.SaveChanges();

            if (currentUser.Role == "Admin")
            {
                return RedirectToAction("AllTickets");
            }

            return RedirectToAction("MyTickets");
        }

     



    }
}
