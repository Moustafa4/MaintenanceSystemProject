using System.ComponentModel.DataAnnotations;
namespace MaintenanceSystem.Models.Entities
{
    public class Device
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string SerialNumber { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        public string AssignedToUserId { get; set; }

        public ApplicationUser AssignedToUser { get; set; }

        public List<Ticket> Tickets { get; set; }
    }
}
