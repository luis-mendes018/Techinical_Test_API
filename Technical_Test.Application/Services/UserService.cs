using Technical_Test.Application.DTOs.UsersDTOs;
using Technical_Test.Application.Interfaces;
using Technical_Test.Domain.Repositories.Interfaces;

namespace Technical_Test.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();

        var userDtos = users.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.Username
        });

        return userDtos;
    }

    public async Task<UserDto> GetUserByIdAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user == null)
        {
            return null; 
        }

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username
        };
    }
    public async Task<bool> UpdateUserAsync(int userId, string newUsername)
    {
        if (string.IsNullOrEmpty(newUsername))
        {
            return false;
        }

        var rowsAffected = await _userRepository.UpdateUserAsync(userId, newUsername);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        var rowsAffected = await _userRepository.DeleteUserAsync(userId);
        return rowsAffected > 0;
    }
}
