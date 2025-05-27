using ShutafimService.Domain.Enums;

namespace ShutafimService.Application.DTO.ListingDTO
{
    public class UpdateListingDto
    {
        public RentalType? RentalType { get; set; }
        public PropertyType? PropertyType { get; set; }
        public ContactMethod? ContactMethod { get; set; }

        public bool? Furnished { get; set; }
        public double? AreaM2 { get; set; }
        public int? NumberOfRooms { get; set; }
        public string? Location { get; set; }
        public decimal? Price { get; set; }

        public List<UtilitiesCovered>? UtilitiesCovered { get; set; }
        public bool? Guarantor { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? PhoneNumber { get; set; }

        public bool? AgentsInvolved { get; set; }
        public bool? OnlineTour { get; set; }

        public int? Floor { get; set; }
        public int? TotalFloors { get; set; }

        public decimal? Deposit { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }

        public ShelterType? Shelter { get; set; }

        public List<string>? RoomsDescription { get; set; }
        public Dictionary<string, int>? RoomDetails { get; set; }
        public Dictionary<string, int>? FurnitureDetails { get; set; }

        public List<string>? Amenities { get; set; }
        public List<string>? Rules { get; set; }

        public string? Description { get; set; }
    }
}
