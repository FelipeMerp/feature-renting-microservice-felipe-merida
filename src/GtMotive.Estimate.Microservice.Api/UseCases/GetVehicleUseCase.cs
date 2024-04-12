using System;
using System.Linq;
using GtMotive.Estimate.Microservice.ApplicationCore.Exceptions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.Domain.Models;
using GtMotive.Estimate.Microservice.Infrastructure.Interfaces;

namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetVehicleUseCase"/> class.
    /// Use case class responsible for retrieving a vehicle by its ID.
    /// </summary>
    public class GetVehicleUseCase : IGetVehicleUseCase
    {
        private readonly IRentingDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetVehicleUseCase"/> class.
        /// Constructor of the use case.
        /// </summary>
        /// <param name="dbContext">The database context for accessing vehicle data.</param>
        public GetVehicleUseCase(IRentingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Executes the retrieval of a vehicle by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to retrieve.</param>
        /// <returns>The result containing the retrieved vehicle.</returns>
        public Vehicle Execute(int id)
        {
            // Find the vehicle in the database by its ID.
            var vehicle = _dbContext.Vehicles.FirstOrDefault(v => v.Id == id && v.ManufacturingDate > DateTime.UtcNow.AddYears(-5)) ?? throw new RentalServiceException("Vehicle not found.");

            // If the vehicle is found, return the vehicle.
            return vehicle;
        }
    }
}
