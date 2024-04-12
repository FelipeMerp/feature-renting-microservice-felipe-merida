using System.Text.Json;
using GtMotive.Estimate.Microservice.ApplicationCore.Exceptions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.Domain.Models;
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
        private readonly IRentVehicleUseCase _rentVehicleUseCase;
        private readonly IReturnVehicleUseCase _returnVehicleUseCase;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalsController"/> class.
        /// Constructor for the RentalsController class.
        /// </summary>
        /// <param name="rentVehicleUseCase">An instance of the rent vehicle use case.</param>
        /// <param name="returnVehicleUseCase">An instance of the return vehicle use case.</param>
        public RentalsController(IRentVehicleUseCase rentVehicleUseCase, IReturnVehicleUseCase returnVehicleUseCase)
        {
            _rentVehicleUseCase = rentVehicleUseCase;
            _returnVehicleUseCase = returnVehicleUseCase;
        }

        /// <summary>
        /// HTTP POST method to rent a vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to be rented.</param>
        /// <param name="jsonRenterId">The JSON object containing the renter ID.</param>
        /// <returns>The result of the rental operation.</returns>
        [HttpPost("rentVehicle/{vehicleId}")]
        public ActionResult<Rental> RentVehicle(int vehicleId, [FromBody] JsonElement jsonRenterId)
        {
            try
            {
                var result = _rentVehicleUseCase.Execute(vehicleId, jsonRenterId);
                return Ok(result);
            }
            catch (RentalServiceException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// HTTP PUT method to return a rented vehicle.
        /// </summary>
        /// <param name="renterId">The ID of the renter returning the vehicle.</param>
        /// <returns>The result of the return operation.</returns>
        [HttpPut("returnVehicle/{renterId}")]
        public ActionResult<Rental> ReturnVehicle(int renterId)
        {
            try
            {
                var result = _returnVehicleUseCase.Execute(renterId);
                return Ok(result);
            }
            catch (RentalServiceException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
