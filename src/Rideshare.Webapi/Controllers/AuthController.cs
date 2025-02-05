using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Features.Auth.CQRS.Commands;
using Rideshare.Application.Features.Auth.Dtos;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Common.Exceptions;
using Rideshare.WebApi.Services;

namespace Rideshare.Webapi.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserAccessor _userAccessor;

    public AuthController(IMediator mediator, IUserAccessor userAccessor)
    {
        _userAccessor = userAccessor;
        _mediator = mediator;
    }

    [HttpPost("admin/login")]
    public async Task<ActionResult> AdminLogin([FromBody] LoginUserDto loginAdminDto)
    {
        var command = new LoginUserCommand { LoginUserDto = loginAdminDto };
        return Ok(await _mediator.Send(command));
    }

    [HttpPost("admin/register")]
    public async Task<ActionResult> AdminRegister([FromBody] RegisterAdminDto registerAdminCommand)
    {
        var command = new RegisterAdminCommand { RegisterAdminDto = registerAdminCommand };
        return Ok(await _mediator.Send(command));
    }

    [HttpPost("rider/login")]
    public async Task<ActionResult> RiderLogin([FromBody] LoginRiderDto loginRiderDto)
    {
        var command = new LoginUserCommand
        {
            LoginUserDto = new LoginUserDto
            {
                PhoneNumber = loginRiderDto.PhoneNumber,
                Password = loginRiderDto.Password
            }
        };

        var response = await _mediator.Send(command);

        if (response.Succeeded)
        {
            return Ok(response);
        }
        else
        {
            return BadRequest(response);
        }
    }

    [HttpPost("rider/register")]
    public async Task<ActionResult> RiderRegister([FromBody] RegisterRiderDto registerRidrDto)
    {
        try
        {
            var command = new RegisterRiderCommand { RegisterRiderDto = registerRidrDto };
            var response = await _mediator.Send(command);
            return Ok(response);
        }
        catch (ConflictException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
        }
    }

    [HttpPost("driver/login")]
    public async Task<ActionResult> DriverLogin([FromBody] LoginDriverDto loginDriverDto)
    {
        var command = new LoginUserCommand
        {
            LoginUserDto = new LoginUserDto
            {
                PhoneNumber = loginDriverDto.PhoneNumber,
                Password = loginDriverDto.Password
            }
        };

        var response = await _mediator.Send(command);

        if (response.Succeeded)
        {
            return Ok(response);
        }
        else
        {
            return BadRequest(response);
        }
    }

    [HttpPost("driver/register")]
    public async Task<ActionResult> DriverRegister([FromBody] RegisterDriverDto registerDriverDto)
    {
        var command = new RegisterDriverCommand { RegisterDriverDto = registerDriverDto };
        return Ok(await _mediator.Send(command));
    }

    [HttpPost("reset-rider-password")]
    public async Task<ActionResult> ResetRiderPassword([FromBody] ResetRiderPasswordDto resetRiderPasswordResponseDto)
    {
        var command = new ResetRiderPasswordCommand { ResetRiderPasswordDto = resetRiderPasswordResponseDto };
        var response = await _mediator.Send(command);

        if (response.Succeeded)
        {
            return Ok(response);
        }
        else
        {
            return BadRequest(response);
        }
    }

    [HttpPost("reset-password/verify-otp")]
    public async Task<ActionResult> ResetPasswordVerify([FromBody] ResetUserPasswordVerifyOTPDto resetUserPasswordVerifyOtpDTO)
    {
        try
        {
            var command = new ResetUserPasswordVerifyOtpCommand { ResetUserPasswordVerifyOtpDTO = resetUserPasswordVerifyOtpDTO };
            var response = await _mediator.Send(command);

            if (response.Succeeded)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
        }
    }

    [HttpPost("reset-password/verify")]
    public async Task<ActionResult> ResetPasswordVerify([FromBody] ResetUserPasswordVerifyDto resetUserPasswordVerifyDto)
    {
        var command = new ResetUserPasswordVerifyCommand { ResetUserPasswordVerifyDto = resetUserPasswordVerifyDto };
        var response = await _mediator.Send(command);

        if (response.Succeeded)
        {
            return Ok(response);
        }
        else
        {
            return BadRequest(response);
        }
    }

    [HttpPost("otp/request-reset-otp")]
    public async Task<ActionResult> ResetPassword([FromBody] RequestOtpDto resetUserPasswordDto)
    {
        var command = new RequestOtpCommand { RequestOtpDto = resetUserPasswordDto };
        var response = await _mediator.Send(command);

        if (response.Succeeded)
        {
            return Ok(response);
        }
        else
        {
            return NotFound(response);
        }
    }

    [HttpPost("otp/request-new-otp")]
    public async Task<ActionResult> RequestOtp([FromBody] RequestOtpDto resetUserPasswordDto)
    {
        var command = new RequestNewOtpCommand { RequestOtpDto = resetUserPasswordDto };
        var response = await _mediator.Send(command);

        if (response.Succeeded)
        {
            return Ok(response);
        }
        else
        {
            return BadRequest(response);
        }
    }

    [HttpPost("otp/verify-phone-number")]
    public async Task<ActionResult> VerifyPhoneNumber([FromBody] VerifyOtpRequest verifyOtpDTO)
    {
        try
        {
            var command = new VerifyOtpCommand { VerifyOtpDto = verifyOtpDTO };
            var response = await _mediator.Send(command);

            if (response.Succeeded)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
        }
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var userId = _userAccessor.GetApplicationUserId();

        if (userId == string.Empty)
        {
            return Unauthorized();
        }

        var command = new RefreshTokenCommand { RefreshTokenRequest = request, UserId = userId };
        var response = await _mediator.Send(command);

        if (response.IsSuccess)
        {
            return Ok(response);
        }
        else
        {
            return BadRequest(response);
        }
    }

    [Authorize]
    [HttpGet("authorized")]
    public ActionResult Authorized()
    {
        return NoContent();
    }
}