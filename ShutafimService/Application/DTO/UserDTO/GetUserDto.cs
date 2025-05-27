namespace ShutafimService.Application.DTO.UserDTO
{
    public class GetUserDto
    {
        public Guid Id { get; set; }
        public string? AvatarUrl { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string? Profession { get; set; }
        public string? Location { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? EmailAddress { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? InterfaceLanguage { get; set; }
        public string? Description { get; set; }
    }

}
