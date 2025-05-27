using Microsoft.AspNetCore.Mvc;
using ShutafimService.Application.DTO;
using ShutafimService.Application.Interfaces;

namespace ShutafimService.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterRequest([FromBody] RegisterRequestDto dto)
        {
            await _authService.SendRegistrationOtpAsync(dto);
            return Ok("OTP sent for registration");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginRequest([FromBody] LoginRequestDto dto)
        {
            var sent = await _authService.SendLoginOtpAsync(dto);
            if (!sent) return BadRequest("User not found");

            return Ok("OTP sent for login");
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyOtpDto dto)
        {
            var token = await _authService.VerifyAndSignInAsync(dto);
            if (token is null) return Unauthorized("Invalid or expired code");

            return Ok(new { token });
        }
    }
}
