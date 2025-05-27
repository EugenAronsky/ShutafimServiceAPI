using ShutafimService.Domain.Enums;

namespace ShutafimService.Application.DTO.ListingDTO
{
    public class ListingFilterDto
    {
        public RentalType? RentalType { get; set; }
        public PropertyType? PropertyType { get; set; }
        public bool? Furnished { get; set; }
        public double? AreaMin { get; set; }
        public double? AreaMax { get; set; }
        public int? NumberOfRoomsMin { get; set; }
        public int? NumberOfRoomsMax { get; set; }
        public string? Location { get; set; }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public List<UtilitiesCovered>? UtilitiesCovered { get; set; }
        public bool? Guarantor { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? RadiusKm { get; set; }
        public bool? AgentsInvolved { get; set; }

        public int? Floor { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }

        public ShelterType? Shelter { get; set; }

        public List<string>? RoomsDescription { get; set; }
        public Dictionary<string, int>? RoomDetails { get; set; }
        public Dictionary<string, int>? FurnitureDetails { get; set; }

        public List<string>? Amenities { get; set; }
        public List<string>? Rules { get; set; }
    }
}
