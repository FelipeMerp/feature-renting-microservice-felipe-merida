using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace GtMotive.Estimate.Microservice.InfrastructureTests
{
    /// <summary>
    /// Tests the functionality of the VehiclesController with valid model data.
    /// </summary>
    public class VehiclesControllerTests : IDisposable
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehiclesControllerTests"/> class.
        /// Constructor.
        /// </summary>
        public VehiclesControllerTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>())
            {
                BaseAddress = new Uri("https://localhost:44366")
            };
            _client = _server.CreateClient();
        }

        /// <summary>
        /// Disposes the resources used by the infrastructure test class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Tests that the CreateVehicle method returns BadRequest when the model is invalid.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous infrastructure test.</returns>
        [Fact]
        public async Task CreateVehicleReturnsBadRequestWhenModelIsInvalidAsync()
        {
            // Arrange
            var requestBody = new
            {
                brand = "Toyota",
                model = "Corolla",
                manufacturingDate = "2010-01-01T00:00:00" // Date more than 5 years old
            };
            var jsonRequest = JsonSerializer.Serialize(requestBody);
            using var httpContent = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(new Uri(_server.BaseAddress, $"/api/vehicles/createVehicle"), httpContent);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        /// <summary>
        /// Disposes the resources used by the infrastructure test class.
        /// </summary>
        /// <param name="disposing">A flag indicating whether or not to dispose managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _client.Dispose();
                _server.Dispose();
            }
        }
    }
}
