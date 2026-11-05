
using CampusPlacementManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusPlacementManagement.Data.Configurations
{
    public class CollegeConfiguration : IEntityTypeConfiguration<College>
    {
        public void Configure(EntityTypeBuilder<College> builder)
        {
            // Table name
            builder.ToTable("Colleges");

            // Primary Key
            builder.HasKey(c => c.CollegeId);

            // College Name
            builder.Property(c => c.CollegeName)
                   .IsRequired()
                   .HasMaxLength(200);

            // College Code
            builder.Property(c => c.CollegeCode)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(c => c.CollegeCode)
                   .IsUnique();

            // University Name
            builder.Property(c => c.UniversityName)
                   .HasMaxLength(200);

            // Official Email
            builder.Property(c => c.OfficialEmail)
                   .IsRequired()
                   .HasMaxLength(255);

            // Phone Number
            builder.Property(c => c.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(20);

            // Address
            builder.Property(c => c.Address)
                   .IsRequired()
                   .HasMaxLength(500);

            // Website
            builder.Property(c => c.Website)
                   .HasMaxLength(255);

            // Status
            builder.Property(c => c.Status)
                   .IsRequired();

            // Created At
            builder.Property(c => c.CreatedAt)
                   .IsRequired();
        }
    }
}
