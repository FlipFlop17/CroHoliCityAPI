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
            var connectionString = "Server=ec2-54-73-22-169.eu-west-1.compute.amazonaws.com;Port=5432;Database=dcm5coup0n2mft;user id=gmsetbvmbliyjq;" +
                "Password=582719ff0900fc2317e3d9ea3bc9d3a6e0494c6f3a78c1de6246907a1155dd6a;sslmode=prefer;";

            // Act
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            // Assert
            Assert.IsTrue(connection.State == ConnectionState.Open);    

        }

    }
}