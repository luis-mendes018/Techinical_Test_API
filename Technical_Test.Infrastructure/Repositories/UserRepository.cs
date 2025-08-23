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

}
