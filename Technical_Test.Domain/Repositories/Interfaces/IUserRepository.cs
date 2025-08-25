using Technical_Test.Domain.Entities;

namespace Technical_Test.Domain.Repositories.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();

    Task<int> UpdateUserAsync(int userId, string newUsername);
    Task<int> DeleteUserAsync(int userId);

    Task<User> GetUserByUsernameAsync(string username);
    Task AddUserAsync(User user);

    Task AddRefreshTokenAsync(RefreshToken token);
    Task<RefreshToken> GetRefreshTokenAsync(string token);
    Task RevokeRefreshTokenAsync(string token);

    Task<User> GetUserByIdAsync(int id);
    Task<IEnumerable<string>> GetUserRolesAsync(int userId);
    Task<int> AddUserToRoleAsync(int userId, string roleName);

    Task<int> RevokeUserRoleByIdAsync(int userId, int roleId);
}
