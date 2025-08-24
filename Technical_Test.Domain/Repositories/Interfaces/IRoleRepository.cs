using Technical_Test.Domain.Entities;

namespace Technical_Test.Domain.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<IEnumerable<Role>> GetAllRolesAsync();
    Task<Role> GetRoleByIdAsync(int id);
    Task<int> CreateRoleAsync(string newName);
    Task UpdateRoleAsync(Role role);
    Task DeleteRoleAsync(int id);
}
