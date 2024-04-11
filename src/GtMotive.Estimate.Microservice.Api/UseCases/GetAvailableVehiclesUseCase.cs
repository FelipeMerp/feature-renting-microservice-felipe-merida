using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Models;
using GtMotive.Estimate.Microservice.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    /// <summary>
    /// Use case class responsible for obtaining the list of vehicles available for renting.
    /// </summary>
    public class GetAvailableVehiclesUseCase
    {
        private readonly RentingDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAvailableVehiclesUseCase"/> class.
        /// Use case class responsible for retrieving all available vehicles.
        /// </summary>
        /// <param name="dbContext">The database context for accessing vehicle data.</param>
        public GetAvailableVehiclesUseCase(RentingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Executes the retrieval of all available vehicles.
        /// </summary>
        /// <returns>The result containing the list of available vehicles.</returns>
        public Task<ActionResult<IEnumerable<Vehicle>>> Execute()
        {
            // Query the database to get all vehicles that are not currently rented.
            var availableVehicles = _dbContext.Vehicles
                .Where(v => !_dbContext.Rentals.Any(r => r.VehicleId == v.Id && r.ReturnDate == null)).ToList().AsEnumerable();

            // Prevent displaying vehicles that are more than 5 years old since their manufacturing date in the fleet.
            availableVehicles = availableVehicles.Where(v => v.ManufacturingDate > DateTime.UtcNow.AddYears(-5)).ToList().AsEnumerable();

            // Return HTTP 200 (OK) response with the list of available vehicles.
            return Task.FromResult<ActionResult<IEnumerable<Vehicle>>>(new OkObjectResult(availableVehicles));
        }
    }
}
