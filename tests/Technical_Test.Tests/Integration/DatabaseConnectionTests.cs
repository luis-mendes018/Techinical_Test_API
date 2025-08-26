using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Technical_Test.Tests.Integration;

public class DatabaseConnectionTests
{
    private readonly string _connectionString;

    public DatabaseConnectionTests()
    {
        var configMock = new Mock<IConfiguration>();

        configMock.SetupGet(c => c["ConnectionStrings:DefaultConnection"])
                  .Returns("Server=localhost\\SQLEXPRESS;Database=WeatherDatabase;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;");

        _connectionString = configMock.Object["ConnectionStrings:DefaultConnection"];
    }

    [Fact]
    public void ConnectionToDatabase_MustOpenAndSuccess()
    {
        using var connection = new SqlConnection(_connectionString);

        Exception ex = Record.Exception(() => connection.Open());

        Assert.Null(ex);
        Assert.Equal(System.Data.ConnectionState.Open, connection.State);

        connection.Close();
        Assert.Equal(System.Data.ConnectionState.Closed, connection.State);
    }
}

