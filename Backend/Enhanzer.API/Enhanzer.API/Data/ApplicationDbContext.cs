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

        public DbSet<LocationDetail> LocationDetails { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

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

            modelBuilder.Entity<PurchaseOrder>(entity =>
            {
                entity.ToTable("Purchase_Orders");

                entity.HasKey(e => e.PurchaseOrderId);

                entity.Property(e => e.PurchaseOrderId)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.OrderDate)
                    .IsRequired();

                entity.Property(e => e.NetAmount)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(e => e.TotalItems)
                    .IsRequired();

                entity.Property(e => e.TotalQuantity)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .IsRequired();

                entity.HasMany(e => e.Items)
                    .WithOne(e => e.PurchaseOrder)
                    .HasForeignKey(e => e.PurchaseOrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PurchaseOrderItem>(entity =>
            {
                entity.ToTable("Purchase_Order_Items");

                entity.HasKey(e => e.PurchaseOrderItemId);

                entity.Property(e => e.PurchaseOrderItemId)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.PurchaseOrderId)
                    .IsRequired();

                entity.Property(e => e.ItemName)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(e => e.Batch)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.StandardCost)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(e => e.StandardPrice)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(e => e.Quantity)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(e => e.FreeQty)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(e => e.Discount)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(e => e.TotalCost)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(e => e.TotalSelling)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
            });
        }
    }
}
