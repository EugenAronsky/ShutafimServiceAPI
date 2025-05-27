using ShutafimService.Application.DTO.ListingDTO;
using ShutafimService.Application.Responses;
using ShutafimService.Domain.Entities;

namespace ShutafimService.Domain.Interfaces
{
    public interface IListingRepository
    {
        Task AddAsync(Listing listing);
        Task<Listing?> GetByIdAsync(int id);
        Task DeleteAsync(Listing listing);
        Task UpdateAsync(Listing listing);
        Task<List<Listing>> GetByUserIdAsync(Guid creatorId, int limit, int offset);
        Task<int> CountByUserIdAsync(Guid creatorId);
        Task IncrementViewsAsync(int listingId);
        Task<(List<Listing>, int)> FilterAsync(ListingFilterDto filters, int limit, int offset);
    }
}
