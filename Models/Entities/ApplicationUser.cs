using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace MaintenanceSystem.Models.Entities;

public class ApplicationUser: IdentityUser
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
    [Required]
    public int DepartmentId { get; set; }

    public Department Department { get; set; }
    public List<Device> Devices { get; set; }

    public List<Ticket> CreatedTickets { get; set; }

    public List<Ticket> AssignedTickets { get; set; }

    public List<Ticket> OwnedTickets { get; set; }
}
