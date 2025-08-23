using Technical_Test.Domain.Entities;

namespace Technical_Test.Domain.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByUsernameAsync(string username);
    Task AddUserAsync(User user);

    Task AddRefreshTokenAsync(RefreshToken token);
    Task<RefreshToken> GetRefreshTokenAsync(string token);
    Task RevokeRefreshTokenAsync(string token);

    Task<User> GetUserByIdAsync(int id);
}
