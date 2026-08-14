using System.ComponentModel.DataAnnotations;
using MaintenanceSystem.Data;
namespace MaintenanceSystem.Models.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        [Required]
        [StringLength(2000)]
        public string Description { get; set; }
        [Required]
        public TicketPriority Priority { get; set; }
        [Required]
        public TicketStatus Status { get; set; }

        public int CreatedByUserId { get; set; }

        public ApplicationUser CreatedByUser { get; set; }

        public int? AssignedTechnicianId { get; set; }

        public ApplicationUser AssignedTechnician { get; set; }
        [Required]
        public int DeviceId { get; set; }

        public Device Device { get; set; }
        [Required]
        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        public int? DeviceOwnerUserId { get; set; }

        public ApplicationUser DeviceOwnerUser { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? AssignedAt { get; set; }

        public DateTime? InProgressAt { get; set; }

        public DateTime? ResolvedAt { get; set; }

        public DateTime? ClosedAt { get; set; }
        public int? CancelledByUserId { get; set; }

        public ApplicationUser CancelledByUser { get; set; }


    }

}
