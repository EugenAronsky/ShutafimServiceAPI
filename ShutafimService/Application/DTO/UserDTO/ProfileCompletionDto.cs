namespace ShutafimService.Application.DTO.UserDTO
{
    public class ProfileCompletionDto
    {
        public int Percentage { get; set; }
        public List<string> MissingFields { get; set; } = new();
    }

}
