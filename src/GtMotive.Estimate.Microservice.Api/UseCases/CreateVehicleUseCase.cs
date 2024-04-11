using System;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Models;
using GtMotive.Estimate.Microservice.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    /// <summary>
    /// Use case class responsible for creating a new vehicle.
    /// </summary>
    public class CreateVehicleUseCase
    {
        private readonly RentingDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateVehicleUseCase"/> class.
        /// Constructor of the use case.
        /// </summary>
        /// <param name="dbContext">The database context for accessing vehicle data.</param>
        public CreateVehicleUseCase(RentingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Executes the creation of a new vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle object to be created.</param>
        /// <returns>The result of the vehicle creation operation.</returns>
        public Task<ActionResult<string>> Execute(Vehicle vehicle)
        {
            // Validate that the vehicle is not null and contains required properties.
            if (vehicle == null)
            {
                return Task.FromResult<ActionResult<string>>(new BadRequestObjectResult(new { message = "The vehicle cannot be null." }));
            }

            if (string.IsNullOrWhiteSpace(vehicle.Brand))
            {
                return Task.FromResult<ActionResult<string>>(new BadRequestObjectResult(new { message = "brand cannot be empty." }));
            }

            if (string.IsNullOrWhiteSpace(vehicle.Model))
            {
                return Task.FromResult<ActionResult<string>>(new BadRequestObjectResult(new { message = "model cannot be empty." }));
            }

            if (vehicle.ManufacturingDate == null)
            {
                return Task.FromResult<ActionResult<string>>(new BadRequestObjectResult(new { message = "manufacturingDate cannot be empty." }));
            }

            // Check if the manufacturing date is valid.
            if (vehicle.ManufacturingDate <= DateTime.UtcNow.AddYears(-5))
            {
                return Task.FromResult<ActionResult<string>>(new BadRequestObjectResult(new
                {
                    message = "Cannot create a vehicle with a manufacturing date over 5 years ago."
                }));
            }

            if (vehicle.ManufacturingDate > DateTime.UtcNow)
            {
                return Task.FromResult<ActionResult<string>>(new BadRequestObjectResult(new
                {
                    message = "Cannot create a vehicle with a future manufacturing date."
                }));
            }

            // Add the vehicle to the database and save changes.
            vehicle.Id = _dbContext.Vehicles.Any() ? _dbContext.Vehicles.Max(x => x.Id) + 1 : 1;
            _dbContext.Vehicles.Add(vehicle);

            if (new GetVehicleUseCase(_dbContext).Execute(vehicle.Id).Result is OkObjectResult okResult)
            {
                // Return HTTP 201 (Created) response with the created vehicle.
                return Task.FromResult<ActionResult<string>>(new OkObjectResult(vehicle));
            }
            else
            {
                return Task.FromResult<ActionResult<string>>(new BadRequestObjectResult(new
                {
                    message = "Unregistered vehicle."
                }));
            }
        }
    }
}
