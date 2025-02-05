using Rideshare.Application.Features.PaymentSystem.Application.Dtos;
using System.Collections.Generic;

namespace Rideshare.Application.Common.Response
{
    public class BaseResponse<T>
    {
        public bool IsSuccess { get; set; }
        public List<string> Error { get; set; } = default!;
        public T Value { get; set; } = default!;
        public string Message { get; set; } = string.Empty;

        public static BaseResponse<T> Success(T value, string message) =>
            new() { Value = value, IsSuccess = true, Message = message };

        public static BaseResponse<T> Failure(string message) =>
            new() { Message = message, IsSuccess = false };

        public static BaseResponse<T> FailureWithError(string message, List<string> errors) =>
            new()
            {
                Message = message,
                Error = errors,
                IsSuccess = false
            };
    }
}