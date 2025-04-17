using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Models;

namespace Warehouse.Data.Context
{
    public class WarehouseDbContext : DbContext
    {
        public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options) { }

        public DbSet<Categories> Categories { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<OrderStatuses> OrderStatuses { get; set; }
        public DbSet<Suppliers> Suppliers { get; set; }
        public DbSet<Cities> Cities { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<OrderProducts> OrderProducts { get; set; }
        public DbSet<RolePermissions> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).HasColumnName("IdCategory");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasColumnName("IdProduct");
                entity.Property(p => p.UnitPrice).HasColumnType("decimal(10, 2)");

                entity.HasOne(p => p.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(p => p.IdCategory);

                entity.HasOne(p => p.Supplier)
                    .WithMany(s => s.Products)
                    .HasForeignKey(p => p.IdSupplier);
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Id).HasColumnName("IdOrder");
                entity.Property(o => o.TotalAmount).HasColumnType("decimal(10, 2)");

                entity.HasOne(o => o.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(o => o.IdUser);

                entity.HasOne(o => o.Status)
                    .WithMany(s => s.Orders)
                    .HasForeignKey(o => o.IdStatus);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).HasColumnName("IdUser");

                entity.HasOne(u => u.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(u => u.IdRole);

                entity.HasMany(u => u.Orders)
                    .WithOne(o => o.User)
                    .HasForeignKey(o => o.IdUser);

                entity.HasOne(u => u.Supplier)
                    .WithMany(s => s.Users)
                    .HasForeignKey(u => u.IdSupplier)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<OrderStatuses>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id).HasColumnName("IdStatus");
            });

            modelBuilder.Entity<Suppliers>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id).HasColumnName("IdSupplier");

                entity.HasOne(s => s.City)
                    .WithMany(c => c.Suppliers)
                    .HasForeignKey(s => s.IdCity);

            });

            modelBuilder.Entity<Cities>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).HasColumnName("IdCity");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id).HasColumnName("IdRole");
            });

            modelBuilder.Entity<Permissions>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasColumnName("IdPermission");
            });

            modelBuilder.Entity<OrderProducts>(entity =>
            {
                entity.HasKey(op => new { op.IdOrder, op.IdProduct });
                entity.Property(op => op.IdOrder).HasColumnName("IdOrder");
                entity.Property(op => op.IdProduct).HasColumnName("IdProduct");
                entity.Property(op => op.UnitPrice).HasColumnType("decimal(10, 2)");

                entity.HasOne(op => op.Order)
                    .WithMany(o => o.OrderProducts)
                    .HasForeignKey(op => op.IdOrder);

                entity.HasOne(op => op.Product)
                    .WithMany(p => p.OrderProducts)
                    .HasForeignKey(op => op.IdProduct);
            });

            modelBuilder.Entity<RolePermissions>(entity =>
            {
                entity.HasKey(rp => new { rp.IdRole, rp.IdPermission });
                entity.Property(rp => rp.IdRole).HasColumnName("IdRole");
                entity.Property(rp => rp.IdPermission).HasColumnName("IdPermission");

                entity.HasOne(rp => rp.Role)
                    .WithMany(r => r.RolePermissions)
                    .HasForeignKey(rp => rp.IdRole);

                entity.HasOne(rp => rp.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(rp => rp.IdPermission);
            });
        }
    }
}