using Enhanzer.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Enhanzer.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
           DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public DbSet<LocationDetail> LocationDetails
        {
            get;
            set;
        }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LocationDetail>(entity =>
            {
                entity.ToTable("Location_Details");

                entity.HasKey(e => e.LocationId);

                entity.Property(e => e.LocationId)
                    .HasColumnName("LocationId")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Location_Code)
                    .HasColumnName("Location_Code")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Location_Name)
                    .HasColumnName("Location_Name")
                    .HasMaxLength(150)
                    .IsRequired();
            });
        }
    
    }
}
