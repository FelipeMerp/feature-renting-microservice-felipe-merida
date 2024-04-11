using System.Text.Json;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases;
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
        private readonly RentVehicleUseCase _rentVehicleUseCase;
        private readonly ReturnVehicleUseCase _returnVehicleUseCase;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalsController"/> class.
        /// Constructor of the controller.
        /// </summary>
        /// <param name="rentVehicleUseCase">An instance of <see cref="RentVehicleUseCase"/> containing the logic for renting vehicles.</param>
        /// <param name="returnVehicleUseCase">An instance of <see cref="ReturnVehicleUseCase"/> containing the logic for returning vehicles.</param>
        public RentalsController(RentVehicleUseCase rentVehicleUseCase, ReturnVehicleUseCase returnVehicleUseCase)
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
            return _rentVehicleUseCase.Execute(vehicleId, jsonRenterId);
        }

        /// <summary>
        /// HTTP PUT method to return a rented vehicle.
        /// </summary>
        /// <param name="renterId">The ID of the renter returning the vehicle.</param>
        /// <returns>The result of the return operation.</returns>
        [HttpPut("returnVehicle/{renterId}")]
        public Task<ActionResult<Rental>> ReturnVehicle(int renterId)
        {
            return _returnVehicleUseCase.ExecuteAsync(renterId);
        }
    }
}
