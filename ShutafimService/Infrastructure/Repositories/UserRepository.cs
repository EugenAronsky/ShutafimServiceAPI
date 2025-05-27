using Microsoft.EntityFrameworkCore;
using ShutafimService.Domain.Entities;
using ShutafimService.Domain.Interfaces;
using ShutafimService.Infrastructure.DbContexts;

namespace ShutafimService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ShutafimDbContext _context;

        public UserRepository(ShutafimDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        public async Task<User?> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }
        public async Task<(List<Listing>, int)> GetFavouritesAsync(Guid clientId, int limit, int offset)
        {
            var query = _context.UserListingFavourites
                .Where(f => f.ClientId == clientId)
                .OrderByDescending(f => f.CreatedAt);

            var totalCount = await query.CountAsync();

            var listings = await query
                .Skip(offset)
                .Take(limit)
                .Include(f => f.Listing)
                .Select(f => f.Listing)
                .ToListAsync();

            return (listings, totalCount);
        }

        public async Task AddFavouriteAsync(Guid clientId, int listingId)
        {
            var listing = await _context.Listings.FindAsync(listingId) ?? throw new Exception();
            var fav = new UserListingFavourite
            {
                ClientId = clientId,
                ListingId = listingId,
                CreatedAt = DateTime.UtcNow,
                Listing = listing
            };

            _context.UserListingFavourites.Add(fav);

            if (listing != null)
            {
                listing.Impressions = (listing.Impressions ?? 0) + 1;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteFavouriteAsync(Guid clientId, int listingId)
        {
            var favourite = await _context.UserListingFavourites
                .FirstOrDefaultAsync(f => f.ClientId == clientId && f.ListingId == listingId) ?? throw new Exception("Favourite not found");

            _context.UserListingFavourites.Remove(favourite);

            var listing = await _context.Listings.FindAsync(listingId);
            if (listing != null)
            {
                listing.Impressions = Math.Max(0, (listing.Impressions ?? 0) - 1);
            }

            await _context.SaveChangesAsync();
        }

    }
}