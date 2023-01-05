﻿using Microsoft.EntityFrameworkCore;

namespace NeighborGoodAPI.Models
{
    public class NGDbContext : DbContext
    {
        public NGDbContext(DbContextOptions<NGDbContext> options) : base(options) { }

        public DbSet<Profile> Profiles { get; set; } = null!;
        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        //public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<ItemCategory> ItemCategories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().Ignore("ImageFile");

            //modelBuilder.Entity<Profile>()
            //    .HasOne<Address>(p => p.Address)
            //    .WithOne(ad => ad.Profile)
            //    .HasForeignKey<Address>(ad => ad.ProfileId);
        }
    }
}
