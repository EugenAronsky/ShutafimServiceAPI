using ShutafimService.Application.DTO.ListingDTO;
using ShutafimService.Application.DTO.UserDTO;
using ShutafimService.Application.Responses;

namespace ShutafimService.Application.Interfaces
{
    public interface IUserService
    {
        Task<GetUserDto?> GetByIdAsync(Guid id);
        Task<List<GetUserDto>> GetAllAsync();
        Task<int> GetCountAsync();
        Task UpdateAsync(Guid id, UpdateUserDto updatedUser);
        Task DeleteAsync(Guid id);
        Task<GetUserDto?> GetByPhoneNumberAsync(string phoneNumber);
        Task<GetUserDto?> CreateAsync(CreateUserDto user);
        Task<ProfileCompletionDto> GetProfileCompletionAsync(Guid userId);
        Task<PagedResult<GetListingDto>> GetFavouritesAsync(Guid userId, int limit, int offset);
        Task AddToFavouritesAsync(Guid clientId, int listingId);
        Task DeleteFromFavouritesAsync(Guid clientId, int listingId);
    }
}
