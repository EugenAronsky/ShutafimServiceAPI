namespace ShutafimService.Application.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public ApiError? Error { get; set; }

        public static ApiResponse<T> SuccessResponse(T data) =>
            new() { Success = true, Data = data };

        public static ApiResponse<T> ErrorResponse(string message) =>
            new() { Success = false, Error = new ApiError { Message = message } };
    }
}
