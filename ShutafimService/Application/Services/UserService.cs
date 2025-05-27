using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShutafimService.Application.DTO.ListingDTO;
using ShutafimService.Application.DTO.UserDTO;
using ShutafimService.Application.Interfaces;
using ShutafimService.Application.Responses;
using ShutafimService.Domain.Entities;
using ShutafimService.Domain.Interfaces;
using ShutafimService.Infrastructure.Repositories;

namespace ShutafimService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<GetUserDto?> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user is null ? null : _mapper.Map<GetUserDto>(user);
        }

        public async Task<List<GetUserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<List<GetUserDto>>(users);
        }

        public async Task<int> GetCountAsync()
        {
            return await _userRepository.GetCountAsync();
        }

        public async Task UpdateAsync(Guid id, UpdateUserDto updatedUser)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser is null)
                throw new Exception("User not found");

            _mapper.Map(updatedUser, existingUser); // Update non-null fields
            await _userRepository.UpdateAsync(existingUser);
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                await _userRepository.DeleteAsync(user);
            }
        }

        public async Task<GetUserDto?> GetByPhoneNumberAsync(string phoneNumber)
        {
            var user = await _userRepository.GetByPhoneNumberAsync(phoneNumber);
            return user is null ? null : _mapper.Map<GetUserDto>(user);
        }

        public async Task<GetUserDto> CreateAsync(CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            user.JoinDate = DateTime.UtcNow; 
            user.IsActiveAccount = true;

            await _userRepository.AddAsync(user);
            return _mapper.Map<GetUserDto>(user);
        }

        public async Task<ProfileCompletionDto> GetProfileCompletionAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            // Define all profile fields you expect
            var totalFields = 14;
            var missing = new List<string>();

            if (string.IsNullOrWhiteSpace(user.AvatarUrl)) missing.Add(nameof(user.AvatarUrl));
            if (string.IsNullOrWhiteSpace(user.FirstName)) missing.Add(nameof(user.FirstName));
            if (string.IsNullOrWhiteSpace(user.LastName)) missing.Add(nameof(user.LastName));
            if (string.IsNullOrWhiteSpace(user.Username)) missing.Add(nameof(user.Username));
            if (string.IsNullOrWhiteSpace(user.Profession)) missing.Add(nameof(user.Profession));
            if (string.IsNullOrWhiteSpace(user.Location)) missing.Add(nameof(user.Location));
            if (user.DateOfBirth == null) missing.Add(nameof(user.DateOfBirth));
            if (!user.EmailIsVerified) missing.Add("EmailIsVerified");
            if (string.IsNullOrWhiteSpace(user.AvatarUrl)) missing.Add(nameof(user.AvatarUrl));
            if (string.IsNullOrWhiteSpace(user.PhoneNumber)) missing.Add(nameof(user.PhoneNumber));
            if (string.IsNullOrWhiteSpace(user.EmailAddress)) missing.Add(nameof(user.EmailAddress));
            if (string.IsNullOrWhiteSpace(user.Gender)) missing.Add(nameof(user.Gender));
            if (string.IsNullOrWhiteSpace(user.InterfaceLanguage)) missing.Add(nameof(user.InterfaceLanguage));
            if (string.IsNullOrWhiteSpace(user.Description)) missing.Add(nameof(user.Description));

            var filledCount = totalFields - missing.Count;
            var percentage = (int)((double)filledCount / totalFields * 100);

            return new ProfileCompletionDto
            {
                Percentage = percentage,
                MissingFields = missing
            };
        }

        public async Task<PagedResult<GetListingDto>> GetFavouritesAsync(Guid clientId, int limit, int offset)
        {
            var (favs, totalCount) = await _userRepository.GetFavouritesAsync(clientId, limit, offset);
            var dtoList = _mapper.Map<List<GetListingDto>>(favs);

            return new PagedResult<GetListingDto>
            {
                Items = dtoList,
                TotalCount = totalCount,
                Page = (offset / limit) + 1,
                PageSize = limit
            };
        }

        public async Task AddToFavouritesAsync(Guid clientId, int listingId)
        {
            await _userRepository.AddFavouriteAsync(clientId, listingId);
        }

        public async Task DeleteFromFavouritesAsync(Guid clientId, int listingId)
        {
            await _userRepository.DeleteFavouriteAsync(clientId, listingId);
        }
    }
}