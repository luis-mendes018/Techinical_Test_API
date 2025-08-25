using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Technical_Test.Domain.Entities;
using Technical_Test.Domain.Repositories.Interfaces;

namespace Technical_Test.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection String not found");
    }

    public async Task AddUserAsync(User user)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var sql = "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash)";
            await connection.ExecuteAsync(sql, new { user.Username, user.PasswordHash });
        }
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var user = await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Username = @Username",
                new { Username = username });

            return user;
        }
    }

    public async Task AddRefreshTokenAsync(RefreshToken token)
    {
        var sql = "INSERT INTO RefreshTokens (Token, ExpirationDate, UserId, IsRevoked) VALUES (@Token, @ExpirationDate, @UserId, @IsRevoked)";
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.ExecuteAsync(sql, token);
        }
    }

    public async Task<RefreshToken> GetRefreshTokenAsync(string token)
    {
        var sql = "SELECT * FROM RefreshTokens WHERE Token = @Token";
        using (var connection = new SqlConnection(_connectionString))
        {
            return await connection.QueryFirstOrDefaultAsync<RefreshToken>(sql, new { Token = token });
        }
    }

    public async Task RevokeRefreshTokenAsync(string token)
    {
        var sql = "UPDATE RefreshTokens SET IsRevoked = 1 WHERE Token = @Token";
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.ExecuteAsync(sql, new { Token = token });
        }
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var sql = "SELECT Id, Username, PasswordHash FROM Users WHERE Id = @Id";
            return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
        }
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(int userId)
    {
        var sql = "SELECT R.Name FROM UserRoles UR INNER JOIN Roles R ON UR.RoleId = R.Id WHERE UR.UserId = @UserId";
        using (var connection = new SqlConnection(_connectionString))
        {
            return await connection.QueryAsync<string>(sql, new { UserId = userId });
        }
    }

    public async Task<int> AddUserToRoleAsync(int userId, string roleName)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {

                    var roleId = await connection.QuerySingleOrDefaultAsync<int?>(
                        "SELECT Id FROM Roles WHERE Name = @RoleName",
                        new { RoleName = roleName },
                        transaction: transaction);


                    if (!roleId.HasValue)
                    {
                        transaction.Rollback();
                        return 0;
                    }

                    var rowsAffected = await connection.ExecuteAsync(
                      "INSERT INTO UserRoles (UserId, RoleId) SELECT @UserId, @RoleId WHERE NOT EXISTS (SELECT 1 FROM UserRoles WHERE UserId = @UserId AND RoleId = @RoleId)",
                    new { UserId = userId, RoleId = roleId.Value },
                    transaction: transaction);


                    transaction.Commit();

                    return rowsAffected;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

    public async Task<int> RevokeUserRoleByIdAsync(int userId, int roleId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var rowsAffected = await connection.ExecuteAsync(
                "DELETE FROM UserRoles WHERE UserId = @UserId AND RoleId = @RoleId",
                new { UserId = userId, RoleId = roleId });

            return rowsAffected;
        }
    }

    public async Task<(IEnumerable<User> Users, int TotalCount)> GetAllUsersAsync(int page, int pageSize)
    {
        var sql = @"
            SELECT * FROM Users
            ORDER BY Id
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY;
            
            SELECT COUNT(Id) FROM Users;";

        using (var connection = new SqlConnection(_connectionString))
        {
            var multi = await connection.QueryMultipleAsync(sql, new
            {
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });

            var users = await multi.ReadAsync<User>();
            var totalCount = await multi.ReadSingleAsync<int>();

            return (users, totalCount);
        }
    }

    public async Task<int> UpdateUserAsync(int userId, string newUsername)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            return await connection.ExecuteAsync(
                "UPDATE Users SET Username = @NewUsername WHERE Id = @UserId",
                new { UserId = userId, NewUsername = newUsername });
        }
    }

    public async Task<int> DeleteUserAsync(int userId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            return await connection.ExecuteAsync(
                "DELETE FROM Users WHERE Id = @UserId",
                new { UserId = userId });
        }
    }

}
