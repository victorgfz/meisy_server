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


    }
}
