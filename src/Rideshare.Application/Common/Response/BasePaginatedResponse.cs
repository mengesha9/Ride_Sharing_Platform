namespace Rideshare.Application.Common.Response;

public class BasePaginatedResponse<T> : BaseResponse<T>
{
  public int Page { get; set; }
  public int PageSize { get; set; }

  public static BasePaginatedResponse<T> Success(T value, string message, int page, int pageSize) =>
      new() { Value = value, IsSuccess = true, Message = message, Page = page, PageSize = pageSize };

  public static BasePaginatedResponse<T> Failure(string message) =>
      new() { Message = message, IsSuccess = false };

  public static BasePaginatedResponse<T> FailureWithError(string message, List<string> errors) =>
      new()
      {
        Message = message,
        Error = errors,
        IsSuccess = false
      };
}
