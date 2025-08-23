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
}
