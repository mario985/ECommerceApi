public class ApiResponseDto<T> 
{
    public bool Success { set; get; }
    public string Message { set; get; }
    public T? Data { set; get; }
    public List<string>? Errors { set; get; }
    public ApiResponseDto(bool success, string message, T data = default, List<string> errors = null)
    {
        Success = success;
        Message = message;
        Data = data;
        Errors = errors ?? new List<string>();
    }
}