using Technical_Test.Domain.Entities;

namespace Technical_Test.Application.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<Role>> GetAllRolesAsync();
    Task<bool> CreateRoleAsync(string name);
    Task<bool> UpdateRoleAsync(int id, string newName);
    Task<bool> DeleteRoleAsync(int id);
}
