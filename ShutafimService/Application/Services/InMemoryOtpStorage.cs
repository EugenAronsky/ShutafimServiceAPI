using Microsoft.Extensions.Caching.Memory;
using ShutafimService.Application.DTO;
using ShutafimService.Application.Interfaces;

namespace ShutafimService.Application.Services
{
    public class InMemoryOtpStorage : IOtpStorage
    {
        private readonly IMemoryCache _cache;

        public InMemoryOtpStorage(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task StoreEntryAsync(string phoneNumber, OtpEntry entry)
        {
            _cache.Set(phoneNumber, entry, TimeSpan.FromMinutes(1.5));
            Console.WriteLine($"[OtpCache] Cached OTP for {phoneNumber}: {entry.Code}");
            return Task.CompletedTask;
        }

        public Task<OtpEntry?> GetEntryAsync(string phoneNumber)
        {
            _cache.TryGetValue(phoneNumber, out OtpEntry? entry);
            Console.WriteLine($"[OtpCache] Retrieved OTP for {phoneNumber}: {entry?.Code}");
            return Task.FromResult(entry);
        }

        public Task RemoveEntryAsync(string phoneNumber)
        {
            _cache.Remove(phoneNumber);
            return Task.CompletedTask;
        }
    }

}
