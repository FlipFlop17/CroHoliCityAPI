using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Npgsql;
using System;
using System.Data;

namespace ApiTests
{
    [TestClass]
    public class ApiTests
    {
        private ServiceProvider _serviceProvider;

        [TestInitialize]
        public void Setup()
        {
            // Create the DI container and configure services
            var services = new ServiceCollection();

        }

        [TestMethod]
        public void ConnectToDatabaseLocal()
        {
            // Arrange
            var connectionString = "Server=localhost;Port=5432;Database=FlipFlop;user id=postgres;sslmode=prefer;";

            // Act
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            // Assert
            Assert.IsTrue(connection.State == ConnectionState.Open);    

        }

    }
}