using EmployeeManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
     : base(options)
    {
    }
    public DbSet<Department> Departments { get; set; }

    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(d => d.Id);

            entity.Property(d => d.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(d => d.Description)
                  .HasMaxLength(250);
        });

        // Employee configuration
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.Email)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.HasIndex(e => e.Email)
                  .IsUnique();

            entity.Property(e => e.Phone)
                  .IsRequired()
                  .HasMaxLength(15);

            entity.Property(e => e.Address)
                  .HasMaxLength(250);

            entity.Property(e => e.JoiningDate)
                  .IsRequired();

            entity.HasOne(e => e.Department)
                  .WithMany()
                  .HasForeignKey(e => e.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

    }
}
