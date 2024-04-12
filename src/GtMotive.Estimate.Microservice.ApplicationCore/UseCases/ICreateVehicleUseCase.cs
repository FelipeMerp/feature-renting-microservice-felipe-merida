using GtMotive.Estimate.Microservice.Domain.Models;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Represents a use case for creating a vehicle.
    /// </summary>
    public interface ICreateVehicleUseCase
    {
        /// <summary>
        /// Executes the use case to create a vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle to be created.</param>
        /// <returns>A <see cref="Vehicle"/> representing the result of the asynchronous operation.</returns>
        Vehicle Execute(Vehicle vehicle);
    }
}
