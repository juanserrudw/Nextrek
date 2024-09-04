using EmployeeManagementNextrek.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace EmployeeManagementNextrek.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<EmployeeRole> EmployeeRoles { get; set; }
        public DbSet<EmployeeAddress> EmployeeAddresses { get; set; }
        public DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
        public DbSet<WorkSchedule> WorkSchedules { get; set; }
        public DbSet<WorkShift> WorkShifts { get; set; }
        public DbSet<ScheduleException> ScheduleExceptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de la relación Department-Employee
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict); // Usa Restrict para evitar eliminar en cascada

            // Configuración de la relación auto-referencial para el Manager en Department
            //modelBuilder.Entity<Department>()
            //    .HasOne(d => d.Manager)
            //    .WithMany()  // Esto indica que no hay una colección inversa
            //    .HasForeignKey(d => d.ManagerID)
            //    .OnDelete(DeleteBehavior.Restrict); // Usa Restrict para evitar problemas de eliminación

            base.OnModelCreating(modelBuilder);
        }
    }
}
