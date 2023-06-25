using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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
            var connectionString = "Server=localhost port=5433 dbname=postgres user=postgres sslmode=prefer connect_timeout=10";

            // Act
            var connection = new SqlConnection(connectionString);
            connection.Open();

            // Assert
            Assert.IsTrue(connection.State == ConnectionState.Open);    

        }

    }
}