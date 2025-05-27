using AutoMapper;
using ShutafimService.Application.DTO.ListingDTO;
using ShutafimService.Application.Interfaces;
using ShutafimService.Application.Responses;
using ShutafimService.Domain.Entities;
using ShutafimService.Domain.Enums;
using ShutafimService.Domain.Interfaces;

namespace ShutafimService.Application.Services
{
    public class ListingService : IListingService
    {
        private readonly IListingRepository _listingRepository;
        private readonly IMapper _mapper;

        public ListingService(IListingRepository listingRepository, IMapper mapper)
        {
            _listingRepository = listingRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateListingAsync(CreateListingDto dto, Guid creatorId)
        {
            var listing = _mapper.Map<Listing>(dto);
            listing.ListedById = creatorId;
            listing.Status = ListingStatus.Drafted;
            listing.CreatedAt = DateTime.UtcNow;
            listing.Views = 0;
            listing.Impressions = 0;

            await _listingRepository.AddAsync(listing);
            return listing.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var listing = await _listingRepository.GetByIdAsync(id);
            if (listing == null)
                throw new Exception("Listing not found");

            await _listingRepository.DeleteAsync(listing);
        }

        public async Task UpdateStatusAsync(int id, ListingStatus status)
        {
            var listing = await _listingRepository.GetByIdAsync(id);
            if (listing == null)
                throw new Exception("Listing not found");

            listing.Status = status;
            await _listingRepository.UpdateAsync(listing);
        }

        public async Task<PagedResult<GetListingDto>> GetByUserIdPagedAsync(Guid creatorId, int limit, int offset)
        {
            var totalCount = await _listingRepository.CountByUserIdAsync(creatorId);
            var listings = await _listingRepository.GetByUserIdAsync(creatorId, limit, offset);
            var dtoList = _mapper.Map<List<GetListingDto>>(listings);

            return new PagedResult<GetListingDto>
            {
                Items = dtoList,
                TotalCount = totalCount,
                Page = (offset / limit) + 1,
                PageSize = limit
            };
        }

        public async Task<GetListingDto> GetByIdAsync(int id)
        {
            var listing = await _listingRepository.GetByIdAsync(id);
            if (listing == null)
                throw new Exception("Listing not found");

            var getListingDto = _mapper.Map<GetListingDto>(listing);
            return getListingDto;
        }

        public async Task<ListingStatsDto> GetStatsAsync(int listingId)
        {
            var listing = await _listingRepository.GetByIdAsync(listingId);
            if (listing == null)
                throw new Exception("Listing not found");

            return new ListingStatsDto
            {
                Views = listing.Views ?? 0,
                Impressions = listing.Impressions ?? 0
            };
        }

        public async Task UpdateListingAsync(int id, UpdateListingDto dto)
        {
            var listing = await _listingRepository.GetByIdAsync(id);
            if (listing == null)
                throw new Exception("Listing not found");

            _mapper.Map(dto, listing); 
            await _listingRepository.UpdateAsync(listing);
        }

        public async Task<PagedResult<GetListingDto>> GetFilteredAsync(ListingFilterDto filters, int limit, int offset)
        {
            var (filtered, totalCount) = await _listingRepository.FilterAsync(filters, limit, offset);
            var mapped = _mapper.Map<List<GetListingDto>>(filtered);

            return new PagedResult<GetListingDto>
            {
                Items = mapped,
                TotalCount = totalCount,
                PageSize = limit,
                Page = (offset / limit) + 1
            };
        }

    }
}
