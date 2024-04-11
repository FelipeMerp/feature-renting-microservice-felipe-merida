using System;
using System.Linq;
using GtMotive.Estimate.Microservice.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetVehicleUseCase"/> class.
    /// Use case class responsible for retrieving a vehicle by its ID.
    /// </summary>
    public class GetVehicleUseCase
    {
        private readonly RentingDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetVehicleUseCase"/> class.
        /// Constructor of the use case.
        /// </summary>
        /// <param name="dbContext">The database context for accessing vehicle data.</param>
        public GetVehicleUseCase(RentingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Executes the retrieval of a vehicle by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to retrieve.</param>
        /// <returns>The result containing the retrieved vehicle.</returns>
        public ActionResult<string> Execute(int id)
        {
            // Find the vehicle in the database by its ID.
            var vehicle = _dbContext.Vehicles.FirstOrDefault(v => v.Id == id && v.ManufacturingDate > DateTime.UtcNow.AddYears(-5));

            // If the vehicle is not found, return HTTP 404 (Not Found) response.
            if (vehicle == null)
            {
                return new ActionResult<string>(new NotFoundObjectResult(new { message = "Vehicle not found." }));
            }

            // If the vehicle is found, return HTTP 200 (OK) response with the vehicle.
            return new OkObjectResult(vehicle);
        }
    }
}
