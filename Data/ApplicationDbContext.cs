using MaintenanceSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
namespace MaintenanceSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            //Users with DEpartments
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentId);

            //Devices with Departments
            modelBuilder.Entity<Device>()
                .HasOne(d => d.Department)
                .WithMany(d => d.Devices)
                .HasForeignKey(d => d.DepartmentId);

            //Devices with users
            modelBuilder.Entity<Device>()
                .HasOne(d => d.AssignedToUser)
                .WithMany(u => u.Devices)
                .HasForeignKey(d => d.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);
            //tickets with users created
            modelBuilder.Entity<Ticket>()
                  .HasOne(t => t.CreatedByUser)
                  .WithMany(u => u.CreatedTickets)
                  .HasForeignKey(t => t.CreatedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);

            //tickets with users technicians

            modelBuilder.Entity<Ticket>()
               .HasOne(t => t.AssignedTechnician)
               .WithMany(u => u.AssignedTickets)
               .HasForeignKey(t => t.AssignedTechnicianId)
               .OnDelete(DeleteBehavior.Restrict);
         
            // User → CancelledTickets
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.CancelledByUser)
                .WithMany(u => u.CancelledTickets)
                .HasForeignKey(t => t.CancelledByUserId)
                .OnDelete(DeleteBehavior.Restrict);


            //User → OwnedTickets
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.DeviceOwnerUser)
                .WithMany(u => u.OwnedTickets)
                .HasForeignKey(t => t.DeviceOwnerUserId)
                .OnDelete(DeleteBehavior.Restrict);
            //Device 1 ───── * Ticket
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Device)
                .WithMany(d => d.Tickets)
                .HasForeignKey(t => t.DeviceId)
                .OnDelete(DeleteBehavior.Restrict);
            //Department 1 ───── * Ticket
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Department)
                .WithMany(d => d.Tickets)
                .HasForeignKey(t => t.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}