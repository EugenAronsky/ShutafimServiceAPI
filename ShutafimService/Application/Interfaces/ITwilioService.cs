namespace ShutafimService.Application.Interfaces
{
    public interface ITwilioService
    {
        Task SendSmsAsync(string phoneNumber, string message);
    }

}
