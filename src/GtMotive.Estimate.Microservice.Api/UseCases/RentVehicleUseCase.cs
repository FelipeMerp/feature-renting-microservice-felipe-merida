using System;
using System.Linq;
using System.Text.Json;
using GtMotive.Estimate.Microservice.ApplicationCore.Exceptions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.Domain.Models;
using GtMotive.Estimate.Microservice.Infrastructure.Interfaces;

namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    /// <summary>
    /// Use case class responsible for renting a vehicle.
    /// </summary>
    public class RentVehicleUseCase : IRentVehicleUseCase
    {
        private readonly IRentingDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentVehicleUseCase"/> class.
        /// Constructor of the use case.
        /// </summary>
        /// <param name="dbContext">The database context for accessing rental and vehicle data.</param>
        public RentVehicleUseCase(IRentingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Executes the rental of a vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to be rented.</param>
        /// <param name="jsonRenterId">The JSON element containing the renter ID.</param>
        /// <returns>The result of the rental operation.</returns>
        public Rental Execute(int vehicleId, JsonElement jsonRenterId)
        {
            // Check if the vehicle is available for rental.
            var isVehicleAvailable = !_dbContext.Rentals.Any(r => r.VehicleId == vehicleId && r.ReturnDate == null)
                && _dbContext.Vehicles.Any(v => v.Id == vehicleId && v.ManufacturingDate > DateTime.UtcNow.AddYears(-5));

            if (!isVehicleAvailable)
            {
                throw new RentalServiceException("The vehicle is not available for rental.");
            }

            var renterId = 0;

            if (jsonRenterId.TryGetProperty("renterId", out var elementRenterId))
            {
                if (!int.TryParse(elementRenterId.GetString(), out renterId))
                {
                    throw new RentalServiceException("Invalid renterId format.");
                }
            }
            else
            {
                throw new RentalServiceException("renterId not provided.");
            }

            // Check if the user already has a rented vehicle.
            if (_dbContext.Rentals.Any(r => r.RenterId == renterId && r.ReturnDate == null))
            {
                throw new RentalServiceException("The user already has a rented vehicle.");
            }

            // Create a new instance of Rental and assign values.
            var rental = new Rental
            {
                Id = _dbContext.Rentals.Any() ? _dbContext.Rentals.Max(x => x.Id) + 1 : 1,
                VehicleId = vehicleId,
                RenterId = renterId,
                RentalDate = DateTime.UtcNow,
                ReturnDate = null
            };

            // Save the new rental to the database.
            _dbContext.Rentals.Add(rental);

            return rental;
        }
    }
}
