using System;
using System.Linq;
using GtMotive.Estimate.Microservice.ApplicationCore.Exceptions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.Domain.Models;
using GtMotive.Estimate.Microservice.Infrastructure.Interfaces;

namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    /// <summary>
    /// Use case class responsible for creating a new vehicle.
    /// </summary>
    public class CreateVehicleUseCase : ICreateVehicleUseCase
    {
        private readonly IRentingDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateVehicleUseCase"/> class.
        /// Constructor of the use case.
        /// </summary>
        /// <param name="dbContext">The database context for accessing vehicle data.</param>
        public CreateVehicleUseCase(IRentingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Executes the creation of a new vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle object to be created.</param>
        /// <returns>The result of the vehicle creation operation.</returns>
        public Vehicle Execute(Vehicle vehicle)
        {
            // Validate that the vehicle is not null and contains required properties.
            if (vehicle == null)
            {
                throw new RentalServiceException("The vehicle cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(vehicle.Brand))
            {
                throw new RentalServiceException("brand cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(vehicle.Model))
            {
                throw new RentalServiceException("model cannot be empty.");
            }

            if (vehicle.ManufacturingDate == null)
            {
                throw new RentalServiceException("manufacturingDate cannot be empty.");
            }

            // Check if the manufacturing date is valid.
            if (vehicle.ManufacturingDate <= DateTime.UtcNow.AddYears(-5))
            {
                throw new RentalServiceException("Cannot create a vehicle with a manufacturing date over 5 years ago.");
            }

            if (vehicle.ManufacturingDate > DateTime.UtcNow)
            {
                throw new RentalServiceException("Cannot create a vehicle with a future manufacturing date.");
            }

            // Add the vehicle to the database and save changes.
            vehicle.Id = _dbContext.Vehicles.Any() ? _dbContext.Vehicles.Max(x => x.Id) + 1 : 1;
            _dbContext.Vehicles.Add(vehicle);

            // Return HTTP 201 (Created) response with the created vehicle.
            return new GetVehicleUseCase(_dbContext).Execute(vehicle.Id);
        }
    }
}
