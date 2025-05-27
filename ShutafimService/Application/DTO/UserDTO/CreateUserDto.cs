namespace ShutafimService.Application.DTO.UserDTO
{
    public class CreateUserDto
    {
        public DateTime JoinDate { get; set; } // не-null

        public string Username { get; set; } = string.Empty; // не-null
        public string PhoneNumber { get; set; } = string.Empty; // не-null

        public bool IsActiveAccount { get; set; }
        public bool PhoneIsVerified { get; set; }

    }
}
