using Microsoft.EntityFrameworkCore;
using ShutafimService.Domain.Entities;

namespace ShutafimService.Infrastructure.DbContexts
{
    public class ShutafimDbContext : DbContext
    {
        public ShutafimDbContext(DbContextOptions<ShutafimDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<UserListingFavourite> UserListingFavourites { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Listing>()
                .Property(l => l.FurnitureDetails)
                .HasColumnType("jsonb");

            modelBuilder.Entity<Listing>()
                .Property(l => l.RoomDetails)
                .HasColumnType("jsonb");

            modelBuilder.Entity<Listing>()
                .Property(l => l.Amenities)
                .HasColumnType("jsonb");

            modelBuilder.Entity<Listing>()
                .Property(l => l.Rules)
                .HasColumnType("jsonb");

            modelBuilder.Entity<UserListingFavourite>()
                .HasKey(x => new { x.ClientId, x.ListingId });

            modelBuilder.Entity<UserListingFavourite>()
                .HasOne(x => x.Listing)
                .WithMany()
                .HasForeignKey(x => x.ListingId);

        }
    }
}
