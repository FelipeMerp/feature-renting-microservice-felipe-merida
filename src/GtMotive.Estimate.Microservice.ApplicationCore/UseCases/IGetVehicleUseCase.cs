using GtMotive.Estimate.Microservice.Domain.Models;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Represents a use case for retrieving a vehicle by its ID.
    /// </summary>
    public interface IGetVehicleUseCase
    {
        /// <summary>
        /// Executes the retrieval of a vehicle by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to retrieve.</param>
        /// <returns>The result containing the retrieved vehicle.</returns>
        Vehicle Execute(int id);
    }
}
