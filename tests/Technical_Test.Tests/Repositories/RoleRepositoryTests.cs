using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Technical_Test.Domain.Entities;
using Technical_Test.Infrastructure.Repositories;

namespace Technical_Test.Tests.Repositories;



public class RoleRepositoryTests
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

        Assert.Throws<InvalidOperationException>(() => new RoleRepository(config));
    }

    [Fact]
    public async Task CreateRoleAsync_DeveEstourarSqlException_SemBancoReal()
    {
        var config = CriarConfig(_fakeConnectionString);
        var repo = new RoleRepository(config);

        await Assert.ThrowsAnyAsync<SqlException>(() => repo.CreateRoleAsync("Administrador"));
    }

    [Fact]
    public async Task GetRoleByIdAsync_DeveEstourarSqlException_SemBancoReal()
    {
        var config = CriarConfig(_fakeConnectionString);
        var repo = new RoleRepository(config);

        await Assert.ThrowsAnyAsync<SqlException>(() => repo.GetRoleByIdAsync(1));
    }

    [Fact]
    public async Task GetAllRolesAsync_DeveEstourarSqlException_SemBancoReal()
    {
        var config = CriarConfig(_fakeConnectionString);
        var repo = new RoleRepository(config);

        await Assert.ThrowsAnyAsync<SqlException>(() => repo.GetAllRolesAsync(1, 10));
    }

    [Fact]
    public async Task UpdateRoleAsync_DeveEstourarSqlException()
    {
        var config = CriarConfig(_fakeConnectionString);
        var repo = new RoleRepository(config);

        await Assert.ThrowsAnyAsync<SqlException>(() => repo.UpdateRoleAsync(new Role { Id = 1, Name = "Teste" }));
    }

    [Fact]
    public async Task DeleteRoleAsync_DeveEstourarSqlException()
    {
        var config = CriarConfig(_fakeConnectionString);
        var repo = new RoleRepository(config);

        await Assert.ThrowsAnyAsync<SqlException>(() => repo.DeleteRoleAsync(1));
    }
}
