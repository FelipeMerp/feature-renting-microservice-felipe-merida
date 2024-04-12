using System.Text.Json;
using GtMotive.Estimate.Microservice.Domain.Models;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Represents a use case for renting a vehicle.
    /// </summary>
    public interface IRentVehicleUseCase
    {
        /// <summary>
        /// Executes the rental of a vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to be rented.</param>
        /// <param name="jsonRenterId">The JSON element containing the renter ID.</param>
        /// <returns>The result of the rental operation.</returns>
        Rental Execute(int vehicleId, JsonElement jsonRenterId);
    }
}
