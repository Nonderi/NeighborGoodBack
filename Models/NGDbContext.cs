using Microsoft.EntityFrameworkCore;

namespace NeighborGoodAPI.Models
{
    public class NGDbContext : DbContext
    {
        public NGDbContext(DbContextOptions<NGDbContext> options) : base(options)
        {
        }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Good> Goods { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Good>().Ignore("File");
        }
    }
}
