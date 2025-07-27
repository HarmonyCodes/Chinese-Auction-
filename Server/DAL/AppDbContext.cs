using Microsoft.EntityFrameworkCore;
using Project.models;
using System.Collections.Generic;

namespace Project.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<GiftCart> GiftCarts { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<GiftWinner> GiftWinners { get; set; }
        public DbSet<PurchaseGift> PurchaseGifts { get; set; }
        public DbSet<GiftCartItem> GiftCartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();
            modelBuilder.Entity<GiftCartItem>()
                .HasKey(gc => new { gc.GiftCartId, gc.GiftId });

            modelBuilder.Entity<PurchaseGift>()
                .HasKey(pg => new { pg.PurchaseId, pg.GiftId });

            modelBuilder.Entity<Gift>()
                .HasOne(g => g.GiftWinner)
                .WithOne(w => w.Gift)
                .HasForeignKey<Project.models.GiftWinner>(w => w.GiftId);
            base.OnModelCreating(modelBuilder);
        }


    }
}
