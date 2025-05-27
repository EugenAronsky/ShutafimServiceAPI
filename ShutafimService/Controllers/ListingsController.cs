using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShutafimService.Application.DTO.ListingDTO;
using ShutafimService.Application.Interfaces;
using ShutafimService.Application.Responses;
using ShutafimService.Application.Extensions;
using ShutafimService.Application.Filters;

namespace ShutafimService.Controllers
{
    [ApiController]
    [Route("listings")]
    [Authorize]
    public class ListingsController : ControllerBase
    {
        private readonly IStorageService _storageService;
        private readonly IListingService _listingService;

        public ListingsController(IStorageService storageService, IListingService listingService)
        {
            _storageService = storageService;
            _listingService = listingService;
        }

        //POST /listings
        [HttpPost]
        public async Task<IActionResult> CreateListing([FromBody] CreateListingDto dto)
        {
            try
            {
                var creatorId = User.GetUserId();
                var id = await _listingService.CreateListingAsync(dto, creatorId);
                return Ok(ApiResponse<object>.SuccessResponse(new { listingId = id }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        // GET /listings/my
        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyListings([FromQuery] int limit = 10, [FromQuery] int offset = 0)
        {
            try
            {
                var creatorId = User.GetUserId();
                var result = await _listingService.GetByUserIdPagedAsync(creatorId, limit, offset);
                return Ok(ApiResponse<PagedResult<GetListingDto>>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        // GET /listings/:id
        [HttpGet("{id:int}")]
        [ServiceFilter(typeof(IncrementViewFilter))] 
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var listing = await _listingService.GetByIdAsync(id);
                return Ok(ApiResponse<GetListingDto>.SuccessResponse(listing));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        } //TODO Avoid double-counting from same user/IP

        //GET /listings
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] ListingFilterDto filters, [FromQuery] int limit = 10, [FromQuery] int offset = 0)
        {
            try
            {
                var result = await _listingService.GetFilteredAsync(filters, limit, offset);
                return Ok(ApiResponse<PagedResult<GetListingDto>>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }


        // POST /listings/{id}/photos
        [HttpPost("{id:int}/photos")]
        public async Task<IActionResult> UploadPhotos(int id, List<IFormFile> files)
        {
            try
            {
                if (files.Count == 0)
                    return BadRequest(ApiResponse<string>.ErrorResponse("No files uploaded"));

                var urls = new List<string>();
                foreach (var file in files)
                {
                    var path = $"listings/{id}/{Guid.NewGuid()}_{file.FileName}";
                    var url = await _storageService.UploadAsync(file, path);
                    urls.Add(url);
                }

                return Ok(ApiResponse<List<string>>.SuccessResponse(urls));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        // GET /listings/{id}/photos
        [HttpGet("{id:int}/photos")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPhotos(int id)
        {
            try
            {
                var urls = await _storageService.ListFilesAsync($"listings/{id}");
                return Ok(ApiResponse<List<string>>.SuccessResponse(urls));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        // DELETE /listings/{id}/photos/{photoFileName}
        [HttpDelete("{id:int}/photos/{photoFileName}")]
        public async Task<IActionResult> DeletePhoto(int id, string photoFileName)
        {
            try
            {
                var fileUrl = $"/uploads/listings/{id}/{photoFileName}";
                await _storageService.DeleteAsync(fileUrl);
                return Ok(ApiResponse<string>.SuccessResponse($"Deleted {photoFileName}"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        // DELETE /listings/:id
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _listingService.DeleteAsync(id);
                return Ok(ApiResponse<string>.SuccessResponse("Deleted"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        // PATCH /listings/:id/status
        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> PatchStatus(int id, [FromBody] PatchListingStatusDto dto)
        {
            try
            {
                await _listingService.UpdateStatusAsync(id, dto.Status);
                return Ok(ApiResponse<string>.SuccessResponse("Status updated"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("{id:int}/stats")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListingStats(int id)
        {
            try
            {
                var stats = await _listingService.GetStatsAsync(id);
                return Ok(ApiResponse<ListingStatsDto>.SuccessResponse(stats));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateListing(int id, [FromBody] UpdateListingDto dto)
        {
            try
            {
                await _listingService.UpdateListingAsync(id, dto);
                return Ok(ApiResponse<string>.SuccessResponse("Listing updated"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

    }
}
