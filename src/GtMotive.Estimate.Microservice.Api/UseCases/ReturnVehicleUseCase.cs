// ReturnVehicleUseCase.cs

using System;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.Domain.Models;
using GtMotive.Estimate.Microservice.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    /// <summary>
    /// Use case class responsible for returning a rented vehicle.
    /// </summary>
    public class ReturnVehicleUseCase : IReturnVehicleUseCase
    {
        private readonly IRentingDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnVehicleUseCase"/> class.
        /// Constructor of the use case.
        /// </summary>
        /// <param name="dbContext">The database context for accessing rental data.</param>
        public ReturnVehicleUseCase(IRentingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Executes the return of a rented vehicle.
        /// </summary>
        /// <param name="renterId">The ID of the renter returning the vehicle.</param>
        /// <returns>The result of the return operation.</returns>
        public Task<ActionResult<Rental>> ExecuteAsync(int renterId)
        {
            // Find the rental in the database.
            var rental = _dbContext.Rentals.FirstOrDefault(r => r.RenterId == renterId && r.ReturnDate == null);
            if (rental == null)
            {
                return Task.FromResult<ActionResult<Rental>>(new NotFoundObjectResult(new { message = "Rental not found." }));
            }

            // With a real database only ReturnDate would be updated.
            _dbContext.Rentals.Remove(rental);

            // Register the return date of the vehicle.
            rental.ReturnDate = DateTime.UtcNow;

            _dbContext.Rentals.Add(rental);

            return Task.FromResult<ActionResult<Rental>>(new OkObjectResult(rental));
        }
    }
}
