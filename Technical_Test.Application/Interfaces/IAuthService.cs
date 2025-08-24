using Technical_Test.Application.DTOs;
using Technical_Test.Domain.Entities;

namespace Technical_Test.Application.Interfaces;

public interface IAuthService
{   
    Task<User> LoginAsync(LoginDto loginDto);
    Task<User> RegisterUserAsync(RegisterDto registerDto);
    Task<string> GenerateRefreshTokenAsync(int userId);
    Task<User> ValidateRefreshTokenAsync(string refreshToken);

    Task RevokeRefreshTokenAsync(string refreshToken);

    Task<IEnumerable<string>> GetUserRolesAsync(int userId);

    Task<bool> AddUserToRoleAsync(int userId, string roleName);
}
