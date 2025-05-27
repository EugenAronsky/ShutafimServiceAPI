using ShutafimService.Domain.Entities;

namespace ShutafimService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<List<User>> GetAllAsync();
        Task<int> GetCountAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<User?> GetByPhoneNumberAsync(string phoneNumber);
        Task<(List<Listing> Listings, int TotalCount)> GetFavouritesAsync(Guid clientId, int limit, int offset);
        Task AddFavouriteAsync(Guid clientId, int listingId);
        Task DeleteFavouriteAsync(Guid clientId, int listingId);
    }
}
