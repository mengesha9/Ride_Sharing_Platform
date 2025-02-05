using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Rideshare.Application.Common.Exceptions;
using Rideshare.Application.Contracts.Identity.ConfigurationModels;
using Rideshare.Application.Contracts.Identity.Models;
using Rideshare.Application.Contracts.Identity.Services;
using Rideshare.Application.Features.Auth.Dtos;
using Rideshare.Domain.Entities;

namespace Rideshare.Infrastructure.Identity.Services;

public class AuthService : IAuthService
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly SignInManager<ApplicationUser> _signInManager;
  private readonly RoleManager<ApplicationRole> _roleManager;
  private readonly JwtSettings _jwtSettings;
  private readonly IDateTimeProvider _dateTimeProvider;

  public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
      IOptions<JwtSettings> jwtSettings, RoleManager<ApplicationRole> roleManager, IDateTimeProvider dateTimeProvider)
  {
    _userManager = userManager;
    _signInManager = signInManager;
    _jwtSettings = jwtSettings.Value;
    _roleManager = roleManager;
    _dateTimeProvider = dateTimeProvider;

  }


  public async Task<LoginUserResponse> LoginUser(LoginUserRequest request)
  {
    var user = await FindApplicationUser(email: request.Email, userName: request.UserName, phoneNumber: request.PhoneNumber);

    if (user is null)
    {
      throw new NotFoundException(nameof(user), request.Email ?? request.UserName ?? request.PhoneNumber ?? "");
    }

    var passwordLogin = await _signInManager.PasswordSignInAsync(
      userName: user.UserName ?? "",
      password: request.Password,
      isPersistent: true,
      lockoutOnFailure: false
    );

    if (!passwordLogin.Succeeded)
    {
      throw new BadRequestException($"Invalid credentials for user: {request.UserName}");
    }

    // if (!user.PhoneNumberConfirmed)
    // {
    //   throw new BadRequestException("Phone number is not verified. Please verify your phone number.");
    // }


    var Token = await GenerateToken(user);
    var RefreshToken = GenerateRefreshToken();

    user.RefreshToken = new RefreshTokenModel
    {
      Token = RefreshToken,
      ExpiresIn = _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpiry)
    };
    await _userManager.UpdateAsync(user);

    return new LoginUserResponse
    {
      Token = Token,
      RefreshToken = RefreshToken
      // User = user
    };
  }

  public async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request)
  {
    if (request.Email is not null && await _userManager.FindByEmailAsync(request.Email) is not null)
    {
      throw new BadRequestException("Email is already used by another user. Please choose another one.");
    }

    if (request.PhoneNumber is not null && FindByPhoneNumber(request.PhoneNumber) is not null)
    {
      throw new BadRequestException("Phone number is already used by another user. Please choose another one.");
    }

    var user = new ApplicationUser
    {
      Email = request.Email,
      PhoneNumber = request.PhoneNumber,
      EmailConfirmed = true,
      PhoneNumberConfirmed = false,
    };

    // Populate other fields based on the provided data
    if (request.FirstName is not null)
    {
      user.FirstName = request.FirstName;
      user.LastName = request.LastName ?? ""; // Assign LastName if provided, it's okay if it's null
      user.FullName = request.LastName != null ? $"{request.FirstName} {request.LastName}" : request.FirstName;
    }
    else if (request.FullName is not null)
    {
      var names = request.FullName.Split(" ");
      user.FullName = request.FullName;
      user.FirstName = names[0]; // Always assign the first part as FirstName
      if (names.Length > 1)
      {
        // Only assign LastName if there's more than one name part
        user.LastName = string.Join(" ", names.Skip(1)); // Join the rest as LastName in case of middle names
      }
    }
    user.UserName = request.UserName ?? request.Email ?? request.PhoneNumber;

    // Create user and populate roles
    var creatingUser = await _userManager.CreateAsync(user, request.Password);
    if (!creatingUser.Succeeded)
    {
      throw new BadRequestException(creatingUser.ToString());
    }

    foreach (var role in request.Roles)
    {
      await CreateRole(role);
      await _userManager.AddToRoleAsync(user, role);
    }

    var Token = await GenerateToken(user);
    var RefreshToken = GenerateRefreshToken();

    user.RefreshToken = new RefreshTokenModel
    {
      Token = RefreshToken,
      ExpiresIn = _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpiry)
    };
    await _userManager.UpdateAsync(user);
    return new RegisterUserResponse
    {
      Token = Token,
      User = user,
      RefreshToken = RefreshToken
    };
  }

  public async Task<bool> DeleteUser(Guid applicationUserId)
  {
    var user = await _userManager.FindByIdAsync(applicationUserId.ToString());
    if (user is null)
    {
      throw new NotFoundException(nameof(user), applicationUserId.ToString());
    }

    var result = await _userManager.DeleteAsync(user);
    return result.Succeeded;
  }
  public async Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request)
  {
    var user = await FindApplicationUser(email: request.Email, userName: request.Username, phoneNumber: request.PhoneNumber);

    if (user is null)
    {
      throw new NotFoundException(nameof(user), request.Email ?? request.Username ?? request.PhoneNumber ?? "");
    }

    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

    return new ResetPasswordResponse
    { Token = token };
  }

  public async Task<ResetPasswordVerifyResponse> ResetPasswordVerify(ResetPasswordVerifyRequest request)
  {
    var user = await FindApplicationUser(email: request.Email, userName: request.Username, phoneNumber: request.PhoneNumber);
    if (user is null)
    {
      throw new NotFoundException(nameof(user), "");
    }

    var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
    if (!result.Succeeded)
    {
      throw new BadRequestException(result.ToString());
    }

    return new ResetPasswordVerifyResponse();
  }

  public async Task CreateRole(string roleName)
  {
    var roleExists = await _roleManager.RoleExistsAsync(roleName);
    if (roleExists)
    {
      return;
    }

    var role = new ApplicationRole
    {
      Name = roleName
    };

    await _roleManager.CreateAsync(role);
  }

  private ApplicationUser? FindByPhoneNumber(string phoneNumber)
  {
    var users = _userManager.Users.Where(user => user.PhoneNumber == phoneNumber);
    if (users.Count() == 0)
    {
      return null;
    }

    return users.First();
  }



  private async Task<string> GenerateToken(ApplicationUser user)
  {
    var userClaims = await _userManager.GetClaimsAsync(user);
    var userRoles = await _userManager.GetRolesAsync(user);
    var userRoleClaims = userRoles.Select(role => new Claim(ClaimTypes.Role, role, "string")).ToList();

    var signingCredentials = new SigningCredentials(
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key ?? "")),
        SecurityAlgorithms.HmacSha256Signature
    );

    var claims = new List<Claim>
      {
          new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
          new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
          new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
      }.Union(userClaims).Union(userRoleClaims);

    var securityToken = new JwtSecurityToken(
        issuer: _jwtSettings.Issuer,
        audience: _jwtSettings.Audience,
        expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
        signingCredentials: signingCredentials,
        claims: claims
    );

    return new JwtSecurityTokenHandler().WriteToken(securityToken);
  }
  public async Task<RegisterUserResponse> RegisterDriver(RegisterUserRequest request)
  {
    var alreadyExist = _userManager.Users.FirstOrDefault(x => x.PhoneNumber == request.PhoneNumber);
    if (alreadyExist is not null)
    {
      throw new BadRequestException("Phone number is already used by another user.");
    }

    var user = new ApplicationUser
    {
      FullName = request.FullName,
      PhoneNumber = request.PhoneNumber,
      UserName = request.PhoneNumber,
    };
    var creatingUser = await _userManager.CreateAsync(user, request.Password);
    if (!creatingUser.Succeeded)
    {
      throw new BadRequestException(creatingUser.ToString());
    }
    foreach (var role in request.Roles)
    {
      await CreateRole(role);
      await _userManager.AddToRoleAsync(user, role);
    }
    return new RegisterUserResponse
    {
      User = user
    };
  }

  public async Task<User?> FindUser(string phoneNumber)
  {
    ApplicationUser? user = null;
    if (phoneNumber is not null)
    {
      user = FindByPhoneNumber(phoneNumber);
    }
    return user;
  }

  private async Task<ApplicationUser?> FindApplicationUser(string? email, string? userName, string? phoneNumber)
  {
    ApplicationUser? user = null;
    if (email is not null)
    {
      user = await _userManager.FindByEmailAsync(email);
    }
    else if (userName is not null)
    {
      user = await _userManager.FindByNameAsync(userName);
    }
    else if (phoneNumber is not null)
    {
      user = FindByPhoneNumber(phoneNumber);
    }

    return user;
  }

  public async Task VerifyPhoneNumber(VerifyPhoneNumberRequest request)
  {
    var user = FindByPhoneNumber(request.PhoneNumber);

    if (user is null)
    {
      throw new NotFoundException(nameof(user), request.PhoneNumber);
    }

    user.PhoneNumberConfirmed = true;
    await _userManager.UpdateAsync(user);
  }

  public async Task DeleteUser(string? email, string? userName, string? phoneNumber)
  {
    var user = await FindApplicationUser(email, userName, phoneNumber);
    if (user is null)
    {
      throw new NotFoundException(nameof(user), email ?? userName ?? phoneNumber ?? "");
    }
    await _userManager.DeleteAsync(user);
  }
  public async Task<User?> FindRider(string phoneNumber)
  {
    ApplicationUser? user = null;
    if (phoneNumber is not null)
    {
      user = FindByPhoneNumber(phoneNumber);
    }
    return user;
  }

  public async Task<ResetRiderPasswordResponse> ResetRiderPassword(ResetRiderPasswordRequest request)
  {
    var user = await FindApplicationUser(email: null, userName: null, phoneNumber: request.PhoneNumber);

    if (user is null)
    {
      throw new NotFoundException(nameof(user), request.PhoneNumber ?? "");
    }

    // Generate password reset token
    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

    // Reset the user password then save in the database
    var resetPasswordResult = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
    if (!resetPasswordResult.Succeeded)
    {
      throw new Exception("Error resetting password.");
    }

    return new ResetRiderPasswordResponse();
  }

  public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request, string userId)
  {
    var user = await _userManager.FindByIdAsync(userId);

    if (user == null || user?.RefreshToken?.Token != request.RefreshToken || user.RefreshToken.ExpiresIn <= _dateTimeProvider.UtcNow)
    {
      throw new BadRequestException("Invalid refresh token.");
    }

    var newToken = await GenerateToken(user);

    return new RefreshTokenResponse
    {
      Token = newToken,
    };
  }


  public string GenerateRefreshToken()
  {
    var randomNumber = new byte[64];
    using (var rng = RandomNumberGenerator.Create())
    {
      rng.GetBytes(randomNumber);
      return Convert.ToBase64String(randomNumber);
    }
  }
}