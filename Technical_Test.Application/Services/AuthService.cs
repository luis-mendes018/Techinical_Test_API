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

    public async Task<string> LoginAsync(LoginDto loginDto)
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

        // TODO: Implementar a geração do JWT
        return await Task.FromResult("token-placeholder-aqui");
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

    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

}
