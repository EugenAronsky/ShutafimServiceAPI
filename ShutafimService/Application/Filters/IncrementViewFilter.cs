using Microsoft.AspNetCore.Mvc.Filters;
using ShutafimService.Domain.Interfaces;

namespace ShutafimService.Application.Filters
{
    public class IncrementViewFilter : IAsyncActionFilter
    {
        private readonly IListingRepository _listingRepository;

        public IncrementViewFilter(IListingRepository listingRepository)
        {
            _listingRepository = listingRepository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("id", out var idObj) && idObj is int listingId)
            {
                await _listingRepository.IncrementViewsAsync(listingId);
            }

            await next(); // proceed with controller action
        }
    }

}
