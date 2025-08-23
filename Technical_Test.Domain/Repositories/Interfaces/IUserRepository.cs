using Technical_Test.Domain.Entities;

namespace Technical_Test.Domain.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByUsernameAsync(string username);
    Task AddUserAsync(User user);
}
