using ShutafimService.Application.DTO;

namespace ShutafimService.Application.Interfaces
{
    public interface IOtpService
    {
        Task SendOtpAsync(string phoneNumber, string purpose, string? username = null, string? firstName = null);
        Task<OtpEntry?> VerifyOtpAsync(string phoneNumber, string code);


    }
}
