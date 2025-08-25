using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Technical_Test.Domain.Entities;
using Technical_Test.Domain.Repositories.Interfaces;

namespace Technical_Test.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly string _connectionString;

    public RoleRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
             ?? throw new InvalidOperationException("Connection String not found");
    }

    public async Task<(IEnumerable<Role> Roles, int TotalCount)> GetAllRolesAsync(int page, int pageSize)
    {
        var sql = @"
            SELECT * FROM Roles
            ORDER BY Id
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY;
            
            SELECT COUNT(Id) FROM Roles;";

        using (var connection = new SqlConnection(_connectionString))
        {
            var multi = await connection.QueryMultipleAsync(sql, new
            {
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });

            var roles = await multi.ReadAsync<Role>();
            var totalCount = await multi.ReadSingleAsync<int>();

            return (roles, totalCount);
        }
    }

    public async Task<Role> GetRoleByIdAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            return await connection.QueryFirstOrDefaultAsync<Role>("SELECT Id, Name FROM Roles WHERE Id = @Id", new { Id = id });
        }
    }

    public async Task<int> CreateRoleAsync(string name)
    {
        var sql = "INSERT INTO Roles (Name) VALUES (@Name); SELECT CAST(SCOPE_IDENTITY() as int)";
        using (var connection = new SqlConnection(_connectionString))
        {
            var newRoleId = await connection.QuerySingleAsync<int>(sql, new { Name = name });
            return newRoleId;
        }
    }

    public async Task UpdateRoleAsync(Role role)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var sql = "UPDATE Roles SET Name = @Name WHERE Id = @Id";

            await connection.ExecuteAsync(sql, role);
        }
    }

    public async Task DeleteRoleAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.ExecuteAsync("DELETE FROM Roles WHERE Id = @Id", new { Id = id });
        }
    }

}
