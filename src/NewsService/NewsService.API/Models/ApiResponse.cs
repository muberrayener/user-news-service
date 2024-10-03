namespace NewsService.NewsService.API.Models
{
    public class AppResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; } 
        public T? Data { get; set; }
        public string ?ErrorCode { get; set; } // For error handling

        public AppResponse()
        {
            Success = true; 
        }

        public static AppResponse<T> CreateSuccessResponse(T data, string message = null)
        {
            return new AppResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static AppResponse<T> CreateErrorResponse(string message, string errorCode = null)
        {
            return new AppResponse<T>
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode
            };
        }
    }
}
