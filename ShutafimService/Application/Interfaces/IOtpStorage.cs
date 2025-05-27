
using ShutafimService.Application.DTO;
namespace ShutafimService.Application.Interfaces
{
    public interface IOtpStorage
    {
        Task StoreEntryAsync(string phoneNumber, OtpEntry entry);
        Task<OtpEntry?> GetEntryAsync(string phoneNumber);
        Task RemoveEntryAsync(string phoneNumber);
    }
}
