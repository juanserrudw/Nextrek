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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Employee - Manager (Self-referencing)
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Manager)
                .WithMany(e => e.Subordinates)
                .HasForeignKey(e => e.ManagerID)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee - Department
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentID);

            // Department - Manager (Self-referencing)
            modelBuilder.Entity<Department>()
                .HasOne(d => d.Manager)
                .WithMany()
                .HasForeignKey(d => d.ManagerID)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee - Role (Many-to-Many)
            modelBuilder.Entity<EmployeeRole>()
                .HasKey(er => new { er.EmployeeID, er.RoleID });

            modelBuilder.Entity<EmployeeRole>()
                .HasOne(er => er.Employee)
                .WithMany(e => e.EmployeeRoles)
                .HasForeignKey(er => er.EmployeeID);

            modelBuilder.Entity<EmployeeRole>()
                .HasOne(er => er.Role)
                .WithMany(r => r.EmployeeRoles)
                .HasForeignKey(er => er.RoleID);

            // Role - Permission (Many-to-Many)
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleID, rp.PermissionID });

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleID);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionID);

            // Employee - Addresses (One-to-Many)
            modelBuilder.Entity<EmployeeAddress>()
                .HasOne(ea => ea.Employee)
                .WithMany(e => e.Addresses)
                .HasForeignKey(ea => ea.EmployeeID);

            // Employee - Documents (One-to-Many)
            modelBuilder.Entity<EmployeeDocument>()
                .HasOne(ed => ed.Employee)
                .WithMany(e => e.Documents)
                .HasForeignKey(ed => ed.EmployeeID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
