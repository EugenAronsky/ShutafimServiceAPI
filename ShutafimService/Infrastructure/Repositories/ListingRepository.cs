using Microsoft.EntityFrameworkCore;
using ShutafimService.Application.DTO.ListingDTO;
using ShutafimService.Domain.Entities;
using ShutafimService.Domain.Interfaces;
using ShutafimService.Infrastructure.DbContexts;

namespace ShutafimService.Infrastructure.Repositories
{
    public class ListingRepository : IListingRepository
    {
        private readonly ShutafimDbContext _context;

        public ListingRepository(ShutafimDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Listing listing)
        {
            await _context.Listings.AddAsync(listing);
            await _context.SaveChangesAsync();
        }

        public async Task<Listing?> GetByIdAsync(int id)
        {
            return await _context.Listings.FindAsync(id);
        }

        public async Task DeleteAsync(Listing listing)
        {
            _context.Listings.Remove(listing);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Listing listing)
        {
            _context.Listings.Update(listing);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Listing>> GetByUserIdAsync(Guid creatorId, int limit, int offset)
        {
            return await _context.Listings
                .Where(l => l.ListedById == creatorId)
                .OrderByDescending(l => l.CreatedAt)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<int> CountByUserIdAsync(Guid creatorId)
        {
            return await _context.Listings.CountAsync(l => l.ListedById == creatorId);
        }

        public async Task IncrementViewsAsync(int listingId)
        {
            var listing = await _context.Listings.FindAsync(listingId);
            if (listing != null)
            {
                listing.Views = (listing.Views ?? 0) + 1;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<(List<Listing>, int)> FilterAsync(ListingFilterDto filters, int limit, int offset)
        {
            var query = _context.Listings.AsQueryable();

            // Enum filters
            if (filters.RentalType.HasValue)
                query = query.Where(l => l.RentalType == filters.RentalType);

            if (filters.PropertyType.HasValue)
                query = query.Where(l => l.PropertyType == filters.PropertyType);

            if (filters.Shelter.HasValue)
                query = query.Where(l => l.Shelter == filters.Shelter);

            if (filters.Furnished.HasValue)
                query = query.Where(l => l.Furnished == filters.Furnished);

            if (filters.Guarantor.HasValue)
                query = query.Where(l => l.Guarantor == filters.Guarantor);

            if (filters.AgentsInvolved.HasValue)
                query = query.Where(l => l.AgentsInvolved == filters.AgentsInvolved);

            // Ranges
            if (filters.AreaMin.HasValue)
                query = query.Where(l => l.AreaM2 >= filters.AreaMin);

            if (filters.AreaMax.HasValue)
                query = query.Where(l => l.AreaM2 <= filters.AreaMax);

            if (filters.PriceMin.HasValue)
                query = query.Where(l => l.Price >= filters.PriceMin);

            if (filters.PriceMax.HasValue)
                query = query.Where(l => l.Price <= filters.PriceMax);

            if (filters.NumberOfRoomsMin.HasValue)
                query = query.Where(l => l.NumberOfRooms >= filters.NumberOfRoomsMin);

            if (filters.NumberOfRoomsMax.HasValue)
                query = query.Where(l => l.NumberOfRooms <= filters.NumberOfRoomsMax);

            if (filters.Floor.HasValue)
                query = query.Where(l => l.Floor == filters.Floor);

            // Dates
            if (filters.CheckInDate.HasValue)
                query = query.Where(l => l.CheckInDate >= filters.CheckInDate);

            if (filters.CheckOutDate.HasValue)
                query = query.Where(l => l.CheckOutDate <= filters.CheckOutDate);

            //Location
            if (filters.Latitude.HasValue && filters.Longitude.HasValue && filters.RadiusKm.HasValue)
            {
                var lat = filters.Latitude.Value;
                var lng = filters.Longitude.Value;
                var radius = filters.RadiusKm.Value;

                var sql = $@"
                  SELECT * FROM ""Listings""
                  WHERE 6371 * acos(
                      cos(radians({lat})) * cos(radians(""Latitude"")) *
                      cos(radians(""Longitude"") - radians({lng})) +
                      sin(radians({lat})) * sin(radians(""Latitude""))
                  ) <= {radius}
                 ";

            }


            // Lists - all must be present
            if (filters.Amenities is { Count: > 0 })
                query = query.Where(l => l.Amenities != null && filters.Amenities.All(a => l.Amenities.Contains(a)));

            if (filters.Rules is { Count: > 0 })
                query = query.Where(l => l.Rules != null && filters.Rules.All(r => l.Rules.Contains(r)));

            if (filters.UtilitiesCovered is { Count: > 0 })
                query = query.Where(l => l.UtilitiesCovered != null && filters.UtilitiesCovered.All(u => l.UtilitiesCovered.Contains(u)));

            // Pagination
            var totalCount = await query.CountAsync();

            var results = await query
                .OrderByDescending(l => l.CreatedAt)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            return (results, totalCount);
        }

    }

}
