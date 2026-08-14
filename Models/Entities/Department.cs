using System.ComponentModel.DataAnnotations;
namespace MaintenanceSystem.Models.Entities
{
    public class Department
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public List<ApplicationUser> Users { get; set; }

        public List<Device> Devices { get; set; }

        public List<Ticket> Tickets { get; set; }
    }
}
