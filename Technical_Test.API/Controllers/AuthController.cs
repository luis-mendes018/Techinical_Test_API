using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Technical_Test.Application.DTOs;
using Technical_Test.Application.Interfaces;

namespace Technical_Test.API.Controllers;

[Route("api/v1/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<LoginDto> _loginDtoValidator;
    private readonly IValidator<RegisterDto> _registerDtoValidator;

    public AuthController(IAuthService authService, IValidator<LoginDto> loginDtoValidator, IValidator<RegisterDto> registerDtoValidator)
    {
        _authService = authService;
        _loginDtoValidator = loginDtoValidator;
        _registerDtoValidator = registerDtoValidator;
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

            var token = await _authService.LoginAsync(loginDto);

            if (token == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            return Ok(new { token });

        }
        catch (Exception)
        {

            return StatusCode(500, "Error processing request");
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
}
