using Rideshare.Domain.Entities;

namespace Rideshare.Application.Contracts.Persistence;

public interface IOtpRepository : IGenericRepository<Otp>
{
  public Task<Otp> GetByPhoneNumber(string phoneNumber);
  public Task<bool> ValidateOtp(string phoneNumber, string otpCode);
  Task DeleteOtp(string phoneNumber);
}
