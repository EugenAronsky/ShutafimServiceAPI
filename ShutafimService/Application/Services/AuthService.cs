using ShutafimService.Application.DTO.UserDTO;
using ShutafimService.Application.DTO;
using ShutafimService.Application.Interfaces;

namespace ShutafimService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IOtpService _otpService;
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(IOtpService otpService, IUserService userService, IJwtTokenService jwtTokenService)
        {
            _otpService = otpService;
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        public async Task SendRegistrationOtpAsync(RegisterRequestDto dto)
        {
            await _otpService.SendOtpAsync(dto.PhoneNumber, "Register", dto.Username);
        }

        public async Task<bool> SendLoginOtpAsync(LoginRequestDto dto)
        {
            var user = await _userService.GetByPhoneNumberAsync(dto.PhoneNumber);
            if (user is null) return false;

            await _otpService.SendOtpAsync(dto.PhoneNumber, "Login");
            return true;
        }

        public async Task<string?> VerifyAndSignInAsync(VerifyOtpDto dto)
        {
            var entry = await _otpService.VerifyOtpAsync(dto.PhoneNumber, dto.Code);
            if (entry is null) return null;

            GetUserDto userDto;

            if (entry.Purpose == "Login")
            {
                userDto = await _userService.GetByPhoneNumberAsync(dto.PhoneNumber);
                if (userDto is null) return null;

                await _userService.UpdateAsync(userDto.Id, new UpdateUserDto
                {
                    LastLogin = DateTime.UtcNow
                });
            }
            else
            {
                userDto = await _userService.GetByPhoneNumberAsync(dto.PhoneNumber)
                           ?? await _userService.CreateAsync(new CreateUserDto
                           {
                               PhoneNumber = dto.PhoneNumber,
                               Username = entry.Username!,
                               JoinDate = DateTime.UtcNow,
                               IsActiveAccount = true,
                               PhoneIsVerified = true
                           });
            }

            return _jwtTokenService.GenerateToken(userDto.Id, userDto.PhoneNumber);
        }
    }

}
