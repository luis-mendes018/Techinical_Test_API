using Technical_Test.Application.DTOs;
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

    public async Task<PagedResultDto<UserDto>> GetAllUsersAsync(int page, int pageSize)
    {
        var (users, totalCount) = await _userRepository.GetAllUsersAsync(page, pageSize);

        var userDtos = users.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.Username
        });

        return new PagedResultDto<UserDto>
        {
            Items = userDtos,
            TotalItems = totalCount,
            CurrentPage = page,
            PageSize = pageSize
        };
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
