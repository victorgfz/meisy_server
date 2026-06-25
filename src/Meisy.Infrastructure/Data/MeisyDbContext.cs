using Meisy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Meisy.Infrastructure.Data
{
    public class MeisyDbContext : DbContext
    {
       public MeisyDbContext(DbContextOptions options) : base(options){}
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Input> Inputs { get; set; }
        public DbSet<Overhead> Overheads { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductInput> Product_Inputs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> Order_Products { get; set; }
        public DbSet<PushSubscription> PushSubscriptions { get; set; }
        public DbSet<Client> Clients { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductInput>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.InputId });

                entity.HasOne(e => e.Product)
                      .WithMany(e => e.ProductInputs)
                      .HasForeignKey(e => e.ProductId);

                entity.HasOne(e => e.Input)
                      .WithMany()
                      .HasForeignKey(e => e.InputId);
            });

            modelBuilder.Entity<PushSubscription>(entity =>
            {
                entity.Property(e => e.Endpoint).HasMaxLength(500);
                entity.Property(e => e.P256DH).HasMaxLength(255);
                entity.Property(e => e.Auth).HasMaxLength(255);
                entity.HasIndex(e => e.Endpoint).IsUnique();

                entity.HasOne(e => e.User)
                      .WithMany(e => e.PushSubscriptions)
                      .HasForeignKey(e => e.UserId);
            });

            modelBuilder.Entity<OrderProduct>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.OrderId });

                entity.HasOne(e => e.Order)
                      .WithMany(e => e.OrderProducts)
                      .HasForeignKey(e => e.OrderId);

                entity.HasOne(e => e.Product)
                      .WithMany()
                      .HasForeignKey(e => e.ProductId);
            });
        }


    }
}
