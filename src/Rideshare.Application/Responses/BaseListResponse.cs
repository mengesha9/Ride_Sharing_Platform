namespace Rideshare.Application.Responses;
public class BaseListResponse<T>
{
    public int TotalCount { get; set; }
    public List<T> Value {get; set;} = new List<T>();
    public bool Succeeded { get; set; } = true;
    public string Message { get; set; } 
    public List<string> Errors { get; set; } = new List<string>();

    public static BaseListResponse<T> Success(List<T> value, int totalCount, string message)
    {
        return new BaseListResponse<T>
        {
            Succeeded = true,
            Message = message,
            Value = value,
            TotalCount = totalCount
        };
    }
    public static BaseListResponse<T> Failure(List<string> errors, string message)
    {
        return new BaseListResponse<T>
        {
            Succeeded = false,
            Message = message,
            Errors = errors
        };
    }
}