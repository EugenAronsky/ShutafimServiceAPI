using ShutafimService.Application.DTO.ListingDTO;
using ShutafimService.Application.Responses;
using ShutafimService.Domain.Enums;

namespace ShutafimService.Application.Interfaces
{
    public interface IListingService
    {
        Task<int> CreateListingAsync(CreateListingDto dto, Guid creatorId);
        Task DeleteAsync(int id);
        Task UpdateStatusAsync(int id, ListingStatus status);
        Task<PagedResult<GetListingDto>> GetByUserIdPagedAsync(Guid creatorId, int limit, int offset);
        Task<GetListingDto> GetByIdAsync(int id);
        Task<ListingStatsDto> GetStatsAsync(int listingId);
        Task UpdateListingAsync(int id, UpdateListingDto dto);
        Task<PagedResult<GetListingDto>> GetFilteredAsync(ListingFilterDto filters, int limit, int offset);

    }
}
