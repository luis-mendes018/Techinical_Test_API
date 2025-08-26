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
        await using (var connection = new SqlConnection(_masterConnectionString))
        {
            var sql = $"SELECT COUNT(*) FROM sys.databases WHERE name = @name";
            var count = await connection.ExecuteScalarAsync<int>(sql, new { name = _databaseName });

            if (count == 0)
            {
                await connection.ExecuteAsync($"CREATE DATABASE {_databaseName}");
            }
        }

        // Criação das tabelas na base de dados
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
                END;

                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
                BEGIN
                    CREATE TABLE Users (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Username NVARCHAR(50) NOT NULL UNIQUE,
                        PasswordHash NVARCHAR(256) NOT NULL
                    );
                END;

                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='RefreshTokens' AND xtype='U')
                BEGIN
                    CREATE TABLE RefreshTokens (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Token NVARCHAR(256) NOT NULL,
                        ExpirationDate DATETIME2 NOT NULL,
                        UserId INT NOT NULL,
                        IsRevoked BIT NOT NULL DEFAULT 0,
                        FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
                    );
                END;

               IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Roles' AND xtype='U')
               BEGIN
                  CREATE TABLE Roles (
                  Id INT IDENTITY(1,1) PRIMARY KEY,
                  Name NVARCHAR(50) NOT NULL UNIQUE
                 );
               END;

              IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserRoles' AND xtype='U')
              BEGIN
                 CREATE TABLE UserRoles (
                   UserId INT NOT NULL,
                   RoleId INT NOT NULL,
                   PRIMARY KEY (UserId, RoleId),
                   FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
                   FOREIGN KEY (RoleId) REFERENCES Roles(Id) ON DELETE CASCADE
                );
               END;";

            await connection.ExecuteAsync(sql);
        }
    }
}
