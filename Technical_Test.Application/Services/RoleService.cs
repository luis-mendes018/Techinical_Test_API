using Technical_Test.Application.Interfaces;
using Technical_Test.Domain.Entities;
using Technical_Test.Domain.Repositories.Interfaces;

namespace Technical_Test.Application.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<IEnumerable<Role>> GetAllRolesAsync()
    {
        return await _roleRepository.GetAllRolesAsync();
    }

    public async Task<Role> GetRoleByIdAsync(int id)
    {
        return await _roleRepository.GetRoleByIdAsync(id);
    }
    public async Task<bool> UpdateRoleAsync(int id, string newName)
    {
        var role = await _roleRepository.GetRoleByIdAsync(id);
        if (role == null)
        {
            return false;
        }
        role.Name = newName;
        await _roleRepository.UpdateRoleAsync(role);
        return true;
    }

    public async Task<bool> DeleteRoleAsync(int id)
    {
        var role = await _roleRepository.GetRoleByIdAsync(id);
        if (role == null)
        {
            return false;
        }
        await _roleRepository.DeleteRoleAsync(id);
        return true;
    }

    public async Task<bool> CreateRoleAsync(string name)
    {
        try
        {
            var roleId = await _roleRepository.CreateRoleAsync(name);
            return roleId > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
