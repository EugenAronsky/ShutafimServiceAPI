using ShutafimService.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ShutafimService.Domain.Entities
{
    public class Listing
    {
        public int Id { get; set; }

        // Required FK
        public Guid ListedById { get; set; }

        // System fields
        public ListingStatus Status { get; set; } = ListingStatus.Drafted;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? Views { get; set; } = 0;
        public int? Impressions { get; set; } = 0;

        // Required fields
        public RentalType RentalType { get; set; }
        public PropertyType PropertyType { get; set; }
        public ContactMethod ContactMethod { get; set; }
        public bool Furnished { get; set; }
        public double AreaM2 { get; set; }
        public int NumberOfRooms { get; set; }
        public string Location { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool Guarantor { get; set; }

        // Optional
        public List<UtilitiesCovered>? UtilitiesCovered { get; set; }
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

        [Range(-90, 90)]
        public double? Latitude { get; set; }

        [Range(-180, 180)]
        public double? Longitude { get; set; }
    }
}
