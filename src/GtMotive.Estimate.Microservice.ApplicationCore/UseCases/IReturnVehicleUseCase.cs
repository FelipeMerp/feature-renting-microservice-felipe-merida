using GtMotive.Estimate.Microservice.Domain.Models;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Represents a use case for returning a rented vehicle.
    /// </summary>
    public interface IReturnVehicleUseCase
    {
        /// <summary>
        /// Executes the return of a rented vehicle.
        /// </summary>
        /// <param name="renterId">The ID of the renter returning the vehicle.</param>
        /// <returns>The result of the return operation.</returns>
        Rental Execute(int renterId);
    }
}
