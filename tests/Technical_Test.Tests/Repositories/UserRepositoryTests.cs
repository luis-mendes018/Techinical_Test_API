using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using Technical_Test.Domain.Entities;
using Technical_Test.Infrastructure.Repositories;

namespace Technical_Test.Tests.Repositories;

public class UserRepositoryTests
{
    private readonly string _fakeConnectionString = "Server=localhost;Database=TestDb;Trusted_Connection=True;";

    private static IConfiguration CriarConfig(string conn)
    {
        var dict = new Dictionary<string, string>();
        if (conn != null)
            dict["ConnectionStrings:DefaultConnection"] = conn;

        return new ConfigurationBuilder()
            .AddInMemoryCollection(dict)
            .Build();
    }

    [Fact]
    public void Ctor_ComConnectionStringNula_DeveLancarExcecao()
    {
        var config = CriarConfig(null);

        Assert.Throws<InvalidOperationException>(() => new UserRepository(config));
    }

    [Fact]
    public async Task AddUserAsync_DeveTentarInserirUsuario_MasEstourarSqlException()
    {
        var config = CriarConfig(_fakeConnectionString);
        var repo = new UserRepository(config);

        var user = new User
        {
            Username = "teste",
            PasswordHash = "hash123"
        };

        await Assert.ThrowsAnyAsync<SqlException>(() => repo.AddUserAsync(user));
    }

    [Fact]
    public async Task GetUserByUsernameAsync_DeveEstourarSqlException_SemBancoReal()
    {
        var config = CriarConfig(_fakeConnectionString);
        var repo = new UserRepository(config);

        await Assert.ThrowsAnyAsync<SqlException>(() => repo.GetUserByUsernameAsync("teste"));
    }

    [Fact]
    public async Task AddRefreshTokenAsync_DeveEstourarSqlException()
    {
        var config = CriarConfig(_fakeConnectionString);
        var repo = new UserRepository(config);

        var token = new RefreshToken
        {
            Token = "abc123",
            ExpirationDate = DateTime.UtcNow.AddDays(7),
            UserId = 1,
            IsRevoked = false
        };

        await Assert.ThrowsAnyAsync<SqlException>(() => repo.AddRefreshTokenAsync(token));
    }

    [Fact]
    public async Task GetAllUsersAsync_ComConnectionValidaMasSemBanco_DeveEstourarSqlException()
    {
        var config = CriarConfig(_fakeConnectionString);
        var repo = new UserRepository(config);

        await Assert.ThrowsAnyAsync<SqlException>(() => repo.GetAllUsersAsync(1, 10));
    }

    [Fact]
    public async Task UpdateUserAsync_DeveEstourarSqlException()
    {
        var config = CriarConfig(_fakeConnectionString);
        var repo = new UserRepository(config);

        await Assert.ThrowsAnyAsync<SqlException>(() => repo.UpdateUserAsync(1, "novoNome"));
    }

    [Fact]
    public async Task DeleteUserAsync_DeveEstourarSqlException()
    {
        var config = CriarConfig(_fakeConnectionString);
        var repo = new UserRepository(config);

        await Assert.ThrowsAnyAsync<SqlException>(() => repo.DeleteUserAsync(1));
    }
}
