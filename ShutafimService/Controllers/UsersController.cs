using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShutafimService.Application.DTO.ListingDTO;
using ShutafimService.Application.DTO.UserDTO;
using ShutafimService.Application.Extensions;
using ShutafimService.Application.Interfaces;
using ShutafimService.Application.Responses;
using ShutafimService.Application.Services;

namespace ShutafimService.Controllers
{

    [ApiController]
    [Route("users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET /users
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                return Ok(ApiResponse<List<GetUserDto>>.SuccessResponse(users));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        // GET /users/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user is null)
                    return NotFound(ApiResponse<string>.ErrorResponse("User not found"));

                return Ok(ApiResponse<GetUserDto>.SuccessResponse(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        // PUT /users/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto updatedUser)
        {
            try
            {
                await _userService.UpdateAsync(id, updatedUser);
                return Ok(ApiResponse<string>.SuccessResponse("User updated"));
            }
            catch (Exception ex)
            {
                return NotFound(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        // DELETE /users/{id}
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _userService.DeleteAsync(id);
                return Ok(ApiResponse<string>.SuccessResponse("User deleted"));
            }
            catch (Exception ex)
            {
                return NotFound(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        // GET /users/me
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userService.GetByIdAsync(userId);
                if (user is null)
                    return NotFound(ApiResponse<string>.ErrorResponse("User not found"));

                return Ok(ApiResponse<GetUserDto>.SuccessResponse(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        // PUT /users/me
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMe([FromBody] UpdateUserDto dto)
        {
            try
            {
                var userId = User.GetUserId();
                await _userService.UpdateAsync(userId, dto);
                return Ok(ApiResponse<string>.SuccessResponse("Profile updated"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        // GET /users/count
        [AllowAnonymous]
        [HttpGet("/users/count")]
        public async Task<IActionResult> GetCount()
        {
            try
            {
                var count = await _userService.GetCountAsync();
                return Ok(ApiResponse<int>.SuccessResponse(count));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("me/profilecompletion")]
        public async Task<IActionResult> GetProfileCompletion()
        {
            try
            {
                var userId = User.GetUserId();
                var result = await _userService.GetProfileCompletionAsync(userId);
                return Ok(ApiResponse<ProfileCompletionDto>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("me/liked-media")]
        [Authorize]
        public async Task<IActionResult> GetFavouritesListings([FromQuery] int limit = 10, [FromQuery] int offset = 0)
        {
            try
            {
                var clientId = User.GetUserId();
                var result = await _userService.GetFavouritesAsync(clientId, limit, offset);
                return Ok(ApiResponse<PagedResult<GetListingDto>>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpPost("me/liked-media/{mediaId:int}")]
        [Authorize]
        public async Task<IActionResult> AddListingToFavourites(int mediaId)
        {
            try
            {
                var clientId = User.GetUserId();
                await _userService.AddToFavouritesAsync(clientId, mediaId);
                return Ok(ApiResponse<string>.SuccessResponse("Added to favourites"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpDelete("me/liked-media/{mediaId:int}")]
        [Authorize]
        public async Task<IActionResult>DeleteFromFavourites(int mediaId)
        {
            try
            {
                var clientId = User.GetUserId();
                await _userService.DeleteFromFavouritesAsync(clientId,mediaId);

                return Ok(ApiResponse<string>.SuccessResponse("Listing deleted from favourites"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }
    }
}
