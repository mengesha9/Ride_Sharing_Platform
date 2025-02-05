using MongoDB.Bson;
using Rideshare.Application.Contracts.Identity.Models;
using Rideshare.Application.Features.Auth.Dtos;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Contracts.Identity.Services;

public interface IAuthService
{
  public Task<LoginUserResponse> LoginUser(LoginUserRequest request);
  public Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request);
  public Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request, string userId);
  public Task<RegisterUserResponse> RegisterDriver(RegisterUserRequest request);
  public Task<bool> DeleteUser(Guid userId);

  public Task VerifyPhoneNumber(VerifyPhoneNumberRequest request);

  public Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request);
  public Task<ResetRiderPasswordResponse> ResetRiderPassword(ResetRiderPasswordRequest request);
  public Task<ResetPasswordVerifyResponse> ResetPasswordVerify(ResetPasswordVerifyRequest request);
  public Task<User?> FindUser(string phoneNumber);
  public Task<User?> FindRider(string phoneNumber);
  public Task CreateRole(string roleName);
}