using Technical_Test.Application.DTOs;
using Technical_Test.Application.Interfaces;
using Technical_Test.Domain.Entities;
using Technical_Test.Domain.Repositories.Interfaces;

namespace Technical_Test.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);
        if (user == null)
        {
            return null;
        }

        var passwordMatches = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
        if (!passwordMatches)
        {
            return null;
        }

        return user;
    }

    public async Task<User> RegisterUserAsync(RegisterDto registerDto)
    {
        try
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(registerDto.Username);
            if (existingUser != null)
            {
                return null;
            }

            
            var passwordHash = HashPassword(registerDto.Password);

            var newUser = new User
            {
                Username = registerDto.Username,
                PasswordHash = passwordHash,
            };

            await _userRepository.AddUserAsync(newUser);
            return newUser;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public async Task<string> GenerateRefreshTokenAsync(int userId)
    {
        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        var refreshToken = new RefreshToken
        {
            Token = token,
            ExpirationDate = DateTime.UtcNow.AddHours(2),
            UserId = userId,
            IsRevoked = false
        };
        await _userRepository.AddRefreshTokenAsync(refreshToken);
        return token;
    }

    public async Task<User> ValidateRefreshTokenAsync(string refreshToken)
    {
        var storedToken = await _userRepository.GetRefreshTokenAsync(refreshToken);

        if (storedToken == null || storedToken.IsRevoked || storedToken.ExpirationDate < DateTime.UtcNow)
        {
            return null;
        }

        var user = await _userRepository.GetUserByIdAsync(storedToken.UserId);
        return user;
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken)
    {
        await _userRepository.RevokeRefreshTokenAsync(refreshToken);
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(int userId)
    {
        return await _userRepository.GetUserRolesAsync(userId);
    }

    public async Task<bool> AddUserToRoleAsync(int userId, string roleName)
    {
        try
        {
            var rowsAffected = await _userRepository.AddUserToRoleAsync(userId, roleName);

            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: {0}", ex.Message);
            return false;
        }
    }

    public async Task<bool> RevokeUserRoleByIdAsync(int userId, int roleId)
    {
        try
        {
            var rowsAffected = await _userRepository.RevokeUserRoleByIdAsync(userId, roleId);
            return rowsAffected > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

}
