using AutoMapper;
using ShutafimService.Application.DTO.ListingDTO;
using ShutafimService.Domain.Entities;

namespace ShutafimService.Application.Mappers
{
    public class ListingProfile : Profile
    {
        public ListingProfile() 
        {
            CreateMap<Listing, GetListingDto>();
            CreateMap<CreateListingDto, Listing>()
              .ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UpdateListingDto, Listing>()
             .ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
