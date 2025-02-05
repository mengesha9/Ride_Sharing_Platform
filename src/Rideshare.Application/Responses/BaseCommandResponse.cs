namespace Rideshare.Application.Responses;

public class BaseCommandResponse<T>
{
  public bool Succeeded { get; set; } = true;
  public string Message { get; set; } = string.Empty;
  public T Value { get; set; } = default!;
  public List<string> Errors { get; set; } = new List<string>();

  public static BaseCommandResponse<T> Success(T value, string message)
  {
    return new BaseCommandResponse<T>
    {
      Succeeded = true,
      Message = message,
      Value = value
    };
  }

  public static BaseCommandResponse<T> Failure(List<string> errors, string message)
  {
    return new BaseCommandResponse<T>
    {
      Succeeded = false,
      Message = message,
      Errors = errors
    };
  }
}

