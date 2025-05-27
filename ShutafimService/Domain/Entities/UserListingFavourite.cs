namespace ShutafimService.Domain.Entities
{
    public class UserListingFavourite
    {
        public Guid ClientId { get; set; }
        public int ListingId { get; set; }
        public Listing Listing { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
