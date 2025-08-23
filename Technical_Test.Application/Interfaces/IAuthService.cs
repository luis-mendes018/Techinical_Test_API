using Technical_Test.Application.DTOs;
using Technical_Test.Domain.Entities;

namespace Technical_Test.Application.Interfaces;

public interface IAuthService
{   
    Task<string> LoginAsync(LoginDto loginDto);
    Task<User> RegisterUserAsync(RegisterDto registerDto);
}
