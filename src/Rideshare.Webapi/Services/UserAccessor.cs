using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MongoDB.Bson;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.WebApi.Services;

public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAdminRepository _adminRepository;
    private readonly IRiderRepository _riderRepository;
    private readonly IDriverRepository _driverRepository;

    public UserAccessor(
      IHttpContextAccessor httpContextAccessor,
      IAdminRepository adminRepository,
      IRiderRepository riderRepository,
      IDriverRepository driverRepository
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _adminRepository = adminRepository;
        _riderRepository = riderRepository;
        _driverRepository = driverRepository;
    }

    public string GetApplicationUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        if (!string.IsNullOrEmpty(token))
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            userId = jwtToken.Subject;
        }

        return userId;
    }

    public ObjectId GetUserId()
    {
        var a = _httpContextAccessor.HttpContext?.User;
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var role = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;

        // Get the expiration time from the JWT
        var expirationClaim = _httpContextAccessor.HttpContext?.User.FindFirst("exp")?.Value;
        DateTime? expirationTime = null;
        if (expirationClaim != null)
        {
            var unixTime = long.Parse(expirationClaim);
            expirationTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;
        }

        // Check if the token is expired
        if (userId is null || role is null || expirationTime is null || expirationTime < DateTime.UtcNow)
        {
            return ObjectId.Empty;
        }

        if (role.CompareTo("Admin") == 0)
        {
            var admin = _adminRepository.GetByApplicationUserId(Guid.Parse(userId)).Result;
            return admin.Id;
        }
        else if (role.CompareTo("Rider") == 0)
        {
            var rider = _riderRepository.GetByApplicationUserId(Guid.Parse(userId)).Result;
            return rider.Id;
        }
        else
        {
            var driver = _driverRepository.GetByApplicationUserId(Guid.Parse(userId)).Result;
            return driver.Id;
        }
    }

}
