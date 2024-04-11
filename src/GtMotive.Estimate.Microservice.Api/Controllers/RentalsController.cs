using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Models;
using GtMotive.Estimate.Microservice.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    /// <summary>
    /// Controller to handle operations related to renting and returning vehicles.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RentalsController : ControllerBase
    {
        private readonly RentingDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalsController"/> class.
        /// Constructor of the controller.
        /// </summary>
        /// <param name="dbContext">Database context for interacting with the Rental entity.</param>
        public RentalsController(RentingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// HTTP POST method to rent a vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to be rented.</param>
        /// <param name="jsonRenterId">The JSON object containing the renter ID.</param>
        /// <returns>The result of the rental operation.</returns>
        [HttpPost("rentVehicle/{vehicleId}")]
        public Task<ActionResult<Rental>> RentVehicle(int vehicleId, [FromBody] JsonElement jsonRenterId)
        {
            // Check if the vehicle is available for rental.
            var isVehicleAvailable = !_dbContext.Rentals.Any(r => r.VehicleId == vehicleId && r.ReturnDate == null)
                && _dbContext.Vehicles.Any(v => v.Id == vehicleId && v.ManufacturingDate > DateTime.UtcNow.AddYears(-5));

            if (!isVehicleAvailable)
            {
                return Task.FromResult<ActionResult<Rental>>(BadRequest(new { message = "The vehicle is not available for rental." }));
            }

            var renterId = 0;

            if (jsonRenterId.TryGetProperty("renterId", out var elementRenterId))
            {
                if (!int.TryParse(elementRenterId.GetString(), out renterId))
                {
                    return Task.FromResult<ActionResult<Rental>>(BadRequest(new { message = "Invalid renterId format." }));
                }
            }
            else
            {
                return Task.FromResult<ActionResult<Rental>>(BadRequest(new { message = "renterId not provided." }));
            }

            // Check if the user already has a rented vehicle.
            if (_dbContext.Rentals.Any(r => r.RenterId == renterId && r.ReturnDate == null))
            {
                return Task.FromResult<ActionResult<Rental>>(BadRequest(new { message = "The user already has a rented vehicle." }));
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

            return Task.FromResult<ActionResult<Rental>>(Ok(rental));
        }

        /// <summary>
        /// HTTP PUT method to return a rented vehicle.
        /// </summary>
        /// <param name="renterId">The ID of the renter returning the vehicle.</param>
        /// <returns>The result of the return operation.</returns>
        [HttpPut("returnVehicle/{renterId}")]
        public Task<ActionResult<Rental>> ReturnVehicle(int renterId)
        {
            // Find the rental in the database.
            var rental = _dbContext.Rentals.FirstOrDefault(r => r.RenterId == renterId && r.ReturnDate == null);
            if (rental == null)
            {
                return Task.FromResult<ActionResult<Rental>>(NotFound(new { message = "Rental not found." }));
            }

            // With a real database only ReturnDate would be updated.
            _dbContext.Rentals.Remove(rental);

            // Register the return date of the vehicle.
            rental.ReturnDate = DateTime.UtcNow;

            _dbContext.Rentals.Add(rental);

            return Task.FromResult<ActionResult<Rental>>(Ok(rental));
        }
    }
}
