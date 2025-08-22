using Dapper;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Technical_Test.Infrastructure.Data;

public class DatabaseInitializer
{
    private readonly string _masterConnectionString;
    private readonly string _connectionString;
    private readonly string _databaseName;

    public DatabaseInitializer(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found");

        _databaseName = new SqlConnectionStringBuilder(_connectionString).InitialCatalog;
        
        _masterConnectionString = _connectionString.Replace(_databaseName, "master");
    }

    public async Task InitializeAsync()
    {
        // Verifique se o banco de dados existe e crie-o se não existir
        await using (var connection = new SqlConnection(_masterConnectionString))
        {
            var sql = $"SELECT COUNT(*) FROM sys.databases WHERE name = @name";
            var count = await connection.ExecuteScalarAsync<int>(sql, new { name = _databaseName });

            if (count == 0)
            {
                await connection.ExecuteAsync($"CREATE DATABASE {_databaseName}");
            }
        }

        // Crie a tabela na nova base de dados
        await using (var connection = new SqlConnection(_connectionString))
        {
            var sql = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='WeatherHistory' AND xtype='U')
                BEGIN
                    CREATE TABLE WeatherHistory (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Lon FLOAT,
                        Lat FLOAT,
                        TempMin FLOAT,
                        TempMax FLOAT,
                        Visibility INT,
                        Sunrise BIGINT,
                        Sunset BIGINT,
                        Description NVARCHAR(255),
                        Main NVARCHAR(255),
                        Speed FLOAT
                    )
                END";

            await connection.ExecuteAsync(sql);
        }
    }
}
