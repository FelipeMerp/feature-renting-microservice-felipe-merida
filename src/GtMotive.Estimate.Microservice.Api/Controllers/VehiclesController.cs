using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Models;
using GtMotive.Estimate.Microservice.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    /// <summary>
    /// Controller to handle operations related to vehicles.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly RentingDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehiclesController"/> class.
        /// Constructor of the controller.
        /// </summary>
        /// <param name="dbContext">Database context for interacting with the Vehicle entity.</param>
        public VehiclesController(RentingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// HTTP POST method to create a new vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle object to be created.</param>
        /// <returns>The result of the vehicle creation operation.</returns>
        [HttpPost("createVehicle")]
        public Task<ActionResult<string>> CreateVehicle([FromBody] Vehicle vehicle)
        {
            // Validate that the vehicle is not null and contains required properties.
            if (vehicle == null)
            {
                return Task.FromResult<ActionResult<string>>(BadRequest(new { message = "The vehicle cannot be null." }));
            }

            if (string.IsNullOrWhiteSpace(vehicle.Brand))
            {
                return Task.FromResult<ActionResult<string>>(BadRequest(new { message = "brand cannot be empty." }));
            }

            if (string.IsNullOrWhiteSpace(vehicle.Model))
            {
                return Task.FromResult<ActionResult<string>>(BadRequest(new { message = "model cannot be empty." }));
            }

            if (vehicle.ManufacturingDate == null)
            {
                return Task.FromResult<ActionResult<string>>(BadRequest(new { message = "manufacturingDate cannot be empty." }));
            }

            // Check if the manufacturing date is valid.
            if (vehicle.ManufacturingDate <= DateTime.UtcNow.AddYears(-5))
            {
                return Task.FromResult<ActionResult<string>>(BadRequest(new
                {
                    message = "Cannot create a vehicle with a manufacturing date over 5 years ago."
                }));
            }

            if (vehicle.ManufacturingDate > DateTime.UtcNow)
            {
                return Task.FromResult<ActionResult<string>>(BadRequest(new
                {
                    message = "Cannot create a vehicle with a future manufacturing date."
                }));
            }

            // Add the vehicle to the database and save changes.
            vehicle.Id = _dbContext.Vehicles.Any() ? _dbContext.Vehicles.Max(x => x.Id) + 1 : 1;
            _dbContext.Vehicles.Add(vehicle);

            // Return HTTP 201 (Created) response with the created vehicle.
            return Task.FromResult<ActionResult<string>>(CreatedAtAction(nameof(GetVehicle), new Vehicle { Id = vehicle.Id }, vehicle));
        }

        /// <summary>
        /// HTTP GET method to retrieve all available vehicles.
        /// </summary>
        /// <returns>The result containing the list of available vehicles.</returns>
        [HttpGet("availableVehicles")]
        public Task<ActionResult<IEnumerable<Vehicle>>> GetAvailableVehicles()
        {
            // Query the database to get all vehicles that are not currently rented.
            var availableVehicles = _dbContext.Vehicles
                .Where(v => !_dbContext.Rentals.Any(r => r.VehicleId == v.Id && r.ReturnDate == null)).ToList().AsEnumerable();

            // Prevent displaying vehicles that are more than 5 years old since their manufacturing date in the fleet.
            availableVehicles = availableVehicles.Where(v => v.ManufacturingDate > DateTime.UtcNow.AddYears(-5)).ToList().AsEnumerable();

            // Return HTTP 200 (OK) response with the list of available vehicles.
            return Task.FromResult<ActionResult<IEnumerable<Vehicle>>>(Ok(availableVehicles));
        }

        /// <summary>
        /// HTTP GET method to retrieve a vehicle by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to retrieve.</param>
        /// <returns>The result containing the retrieved vehicle.</returns>
        [HttpGet("getVehicleById/{id}")]
        public ActionResult<string> GetVehicle(int id)
        {
            // Find the vehicle in the database by its ID.
            var vehicle = _dbContext.Vehicles.FirstOrDefault(v => v.Id == id && v.ManufacturingDate > DateTime.UtcNow.AddYears(-5));

            // If the vehicle is not found, return HTTP 404 (Not Found) response.
            if (vehicle == null)
            {
                return NotFound(new { message = "Vehicle not found." });
            }

            // If the vehicle is found, return HTTP 200 (OK) response with the vehicle.
            return Ok(vehicle);
        }
    }
}
