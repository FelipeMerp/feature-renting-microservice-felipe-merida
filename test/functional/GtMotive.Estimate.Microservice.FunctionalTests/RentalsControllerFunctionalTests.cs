using System.Text.Json;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Controllers;
using GtMotive.Estimate.Microservice.Domain.Models;
using GtMotive.Estimate.Microservice.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Controllers
{
    /// <summary>
    /// Integration tests for the RentalsController class.
    /// </summary>
    public class RentalsControllerFunctionalTests
    {
        private readonly RentingDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalsControllerFunctionalTests"/> class.
        /// Constructor.
        /// </summary>
        public RentalsControllerFunctionalTests()
        {
            _dbContext = new RentingDbContext();
        }

        /// <summary>
        /// Tests the ReturnVehicle method of the RentalsController class.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous functional test.</returns>
        [Fact]
        public async Task ReturnVehicleReturnsOkWhenSuccessful()
        {
            // Arrange
            _dbContext.Rentals.Clear();
            var controller = new RentalsController(_dbContext);

            var renterId = 1;
            var vehicleId = 1;
            var jsonRenterId = JsonSerializer.SerializeToElement(new { renterId = "1" });

            // Act
            var responseRentVehicle = await controller.RentVehicle(vehicleId, jsonRenterId);
            var rentalBefore = (responseRentVehicle.Result as OkObjectResult).Value as Rental;

            var responseReturnVehicle = await controller.ReturnVehicle(renterId);
            var rentalAfter = (responseReturnVehicle.Result as OkObjectResult).Value as Rental;

            // Assert
            Assert.NotNull(rentalBefore);
            Assert.NotNull(rentalAfter);
            rentalBefore.ReturnDate = rentalAfter.ReturnDate;
            Assert.Equal(vehicleId, rentalBefore.VehicleId);
            Assert.Equal(renterId, rentalBefore.RenterId);
            Assert.Equal(vehicleId, rentalAfter.VehicleId);
            Assert.Equal(renterId, rentalAfter.RenterId);
            Assert.Equal(rentalBefore, rentalAfter);
        }
    }
}
