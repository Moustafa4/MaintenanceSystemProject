using MaintenanceSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;
namespace MaintenanceSystem.Models.Entities;

public class ApplicationUser
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string FullName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public Role Role { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

   
    public int? DepartmentId { get; set; }

    public Department Department { get; set; }

    public List<Device> Devices { get; set; }

    public List<Ticket> CreatedTickets { get; set; }

    public List<Ticket> AssignedTickets { get; set; }

    public List<Ticket> OwnedTickets { get; set; }
}
