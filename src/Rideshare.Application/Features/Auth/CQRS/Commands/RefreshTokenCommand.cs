namespace Rideshare.Application.Features.Auth.CQRS.Commands
{
  using MediatR;
  using MongoDB.Bson;
  using Rideshare.Application.Common.Response;
  using Rideshare.Application.Features.Auth.Dtos;
  using Rideshare.Application.Responses;

  public class RefreshTokenCommand : IRequest<BaseResponse<RefreshTokenResponse>>
  {
    public RefreshTokenRequest RefreshTokenRequest { get; set; } = null!;
    public string UserId { get; set; }
  }
}