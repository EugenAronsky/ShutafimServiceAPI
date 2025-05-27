using ShutafimService.Application.DTO;

namespace ShutafimService.Application.Interfaces
{
    public interface IAuthService
    {
        Task SendRegistrationOtpAsync(RegisterRequestDto dto);
        Task<bool> SendLoginOtpAsync(LoginRequestDto dto);
        Task<string?> VerifyAndSignInAsync(VerifyOtpDto dto);
    }
}
