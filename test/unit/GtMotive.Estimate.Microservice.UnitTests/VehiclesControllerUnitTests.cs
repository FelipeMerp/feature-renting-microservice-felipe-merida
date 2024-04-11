using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Controllers;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.Domain.Models;
using GtMotive.Estimate.Microservice.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests
{
    /// <summary>
    /// Unit tests for the VehiclesController class.
    /// </summary>
    public class VehiclesControllerUnitTests
    {
        /// <summary>
        /// Tests the GetAvailableVehicles method of the VehiclesController class.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetAvailableVehiclesReturnsOkWithListOfAvailableVehicles()
        {
            // Arrange
            var dbContextMock = new Mock<RentingDbContext>(); // Mocking the database context
            var controller = new VehiclesController(new CreateVehicleUseCase(dbContextMock.Object), new GetAvailableVehiclesUseCase(dbContextMock.Object), new GetVehicleUseCase(dbContextMock.Object)); // Creating controller with mock context

            // Mocking the Vehicles and Rentals collections
            dbContextMock.Object.Vehicles.Clear();
            dbContextMock.Object.Vehicles.Add(new Vehicle { Id = 1, Brand = "Toyota", Model = "Camry", ManufacturingDate = DateTime.UtcNow.AddYears(-3) });
            dbContextMock.Object.Vehicles.Add(new Vehicle { Id = 2, Brand = "Honda", Model = "Civic", ManufacturingDate = DateTime.UtcNow.AddYears(-2) });
            dbContextMock.Object.Vehicles.Add(new Vehicle { Id = 3, Brand = "Ford", Model = "Focus", ManufacturingDate = DateTime.UtcNow.AddYears(-6) }); // Older than 5 years

            dbContextMock.Object.Rentals.Clear();
            dbContextMock.Object.Rentals.Add(new Rental { Id = 1, VehicleId = 1, RentalDate = DateTime.UtcNow.AddDays(-2), ReturnDate = null }); // Rented
            dbContextMock.Object.Rentals.Add(new Rental { Id = 2, VehicleId = 3, RentalDate = DateTime.UtcNow.AddDays(-5), ReturnDate = DateTime.UtcNow.AddDays(-2) });

            // Act
            var result = await controller.GetAvailableVehicles();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result); // Expects Ok response
            var vehicles = Assert.IsAssignableFrom<IEnumerable<Vehicle>>(okResult.Value); // Expects IEnumerable<Vehicle>
            Assert.Single((List<Vehicle>)vehicles); // Expects only 1 vehicle (not rented and less than 5 years old)
        }
    }
}
