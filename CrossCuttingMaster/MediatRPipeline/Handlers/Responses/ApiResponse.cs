namespace CrossCuttingMaster.MediatRPipeline.Handlers.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public T? Data { get; set; }
        public int ErrorCode { get; set; }
        public List<string>? Errors { get; set; }
        public static ApiResponse<T> Ok(T data) => new() { Data = data };
        public static ApiResponse<T> Fail(int errorCode, List<string> errors) => new() { Success = false, ErrorCode = errorCode, Errors = errors };
    }
}
