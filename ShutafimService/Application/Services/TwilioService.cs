using ShutafimService.Application.Interfaces;

namespace ShutafimService.Application.Services
{
    public class TwilioService : ITwilioService
    {
        public Task SendSmsAsync(string phoneNumber, string message)
        {
            Console.WriteLine($"[TwilioStub] Sending SMS to {phoneNumber}: {message}");
            return Task.CompletedTask;
        }
    }

}
