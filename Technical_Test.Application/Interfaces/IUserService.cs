using Technical_Test.Application.DTOs;
using Technical_Test.Application.DTOs.UsersDTOs;

namespace Technical_Test.Application.Interfaces;

public interface IUserService
{
    Task<PagedResultDto<UserDto>> GetAllUsersAsync(int page, int pageSize);
    Task<UserDto> GetUserByIdAsync(int userId);
    Task<bool> UpdateUserAsync(int userId, string newUsername);
    Task<bool> DeleteUserAsync(int userId);
}
