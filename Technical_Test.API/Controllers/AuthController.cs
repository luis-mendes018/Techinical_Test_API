using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Technical_Test.Application.DTOs;
using Technical_Test.Application.Interfaces;
using Technical_Test.Domain.Services.Interfaces;

namespace Technical_Test.API.Controllers;

[Route("api/v1/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;
    private readonly IValidator<LoginDto> _loginDtoValidator;
    private readonly IValidator<RegisterDto> _registerDtoValidator;

    public AuthController(IAuthService authService, IValidator<LoginDto> loginDtoValidator, IValidator<RegisterDto> registerDtoValidator, ITokenService tokenService)
    {
        _authService = authService;
        _loginDtoValidator = loginDtoValidator;
        _registerDtoValidator = registerDtoValidator;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var validationResult = await _loginDtoValidator.ValidateAsync(loginDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = await _authService.LoginAsync(loginDto);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            var roles = await _authService.GetUserRolesAsync(user.Id);
            var token = _tokenService.GenerateJwtToken(user, roles);
            var refreshToken = await _authService.GenerateRefreshTokenAsync(user.Id);

            return Ok(new { AccessToken = token, RefreshToken = refreshToken });

        }
        catch (Exception ex)
        {

            Console.WriteLine("Error log: {0} ", ex);
            return StatusCode(500, "Error processing request");

        }
    }

    [HttpPost("assign-role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto assignRoleDto)
    {
        try
        {
            var success = await _authService.AddUserToRoleAsync(assignRoleDto.UserId, assignRoleDto.RoleName);
            if (!success)
            {
                return BadRequest("Failed to assign role. User or role may not exist.");
            }

            return Ok("Role assigned successfully.");
        }
        catch (Exception)
        {
            return StatusCode(500, "Error processing the request");
        }

    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            var validationResult = await _registerDtoValidator.ValidateAsync(registerDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var registrationResult = await _authService.RegisterUserAsync(registerDto);

            if (registrationResult == null)
            {
                return Conflict(new { message = "Username already exists." });
            }

            return Ok(new { message = "User registered successfully." });

        }
        catch (Exception)
        {
            return StatusCode(500, "Error processing the request");
        }

    }


    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        var user = await _authService.ValidateRefreshTokenAsync(refreshToken);
        if (user == null)
        {
            return Unauthorized(new { message = "Invalid refresh token." });
        }

        var roles = await _authService.GetUserRolesAsync(user.Id);

        var newAccessToken = _tokenService.GenerateJwtToken(user, roles);

        await _authService.RevokeRefreshTokenAsync(refreshToken);

        var newRefreshToken = await _authService.GenerateRefreshTokenAsync(user.Id);

        return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
    }

}
