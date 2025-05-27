namespace ShutafimService.Application.DTO
{
    public class OtpEntry
    {
        public string Code { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public string Purpose { get; set; } = default!; // "Register" | "Login"
        public string? Username { get; set; }
    }

}
