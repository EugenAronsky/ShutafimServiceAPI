using ShutafimService.Application.DTO;
using ShutafimService.Application.Interfaces;

namespace ShutafimService.Application.Services
{
    public class OtpService : IOtpService
    {
        private readonly IOtpStorage _otpStorage;
        private readonly ITwilioService _twilioService;

        public OtpService(IOtpStorage otpStorage, ITwilioService twilioService)
        {
            _otpStorage = otpStorage;
            _twilioService = twilioService;
        }

        public async Task SendOtpAsync(string phoneNumber, string purpose, string? username = null, string? firstName = null)
        {
            var code = new Random().Next(100000, 999999).ToString();
            var entry = new OtpEntry
            {
                Code = code,
                Purpose = purpose,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                Username = username
            };

            await _otpStorage.StoreEntryAsync(phoneNumber, entry);
            await _twilioService.SendSmsAsync(phoneNumber, $"Your code is {code}");
        }

        public async Task<OtpEntry?> VerifyOtpAsync(string phoneNumber, string code)
        {
            var entry = await _otpStorage.GetEntryAsync(phoneNumber);
            if (entry is null || entry.Code != code || entry.ExpiresAt < DateTime.UtcNow)
                return null;

            await _otpStorage.RemoveEntryAsync(phoneNumber);
            return entry;
        }
    }

}
