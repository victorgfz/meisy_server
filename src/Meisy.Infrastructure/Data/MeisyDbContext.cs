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
        }


    }
}
