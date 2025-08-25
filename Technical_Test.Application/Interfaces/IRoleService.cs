using Technical_Test.Application.DTOs;
using Technical_Test.Domain.Entities;

namespace Technical_Test.Application.Interfaces;

public interface IRoleService
{
    Task<PagedResultDto<Role>> GetAllRolesAsync(int page, int pageSize);
    Task<Role> GetRoleByIdAsync(int id);
    Task<bool> CreateRoleAsync(string name);
    Task<bool> UpdateRoleAsync(int id, string newName);
    Task<bool> DeleteRoleAsync(int id);
}
