
using EFCorePractice.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCorePractice.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Department> Departments => Set<Department>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                     @"Server=.\SQLEXPRESS;Database=StudentCourseDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
    }
}
