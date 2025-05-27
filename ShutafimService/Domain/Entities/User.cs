namespace ShutafimService.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string? AvatarUrl { get; set; }
        public DateTime JoinDate { get; set; } // не-null
        public DateTime? LastLogin { get; set; }

        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public string? Profession { get; set; }
        public string? Location { get; set; }

        public string Username { get; set; } = string.Empty; // не-null
        public string PhoneNumber { get; set; } = string.Empty; // не-null
        public string? EmailAddress { get; set; }

        public bool PhoneIsVerified { get; set; }
        public bool EmailIsVerified { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? InterfaceLanguage { get; set; }
        public string? Description { get; set; }

        public bool IsActiveAccount { get; set; }
    }

}
