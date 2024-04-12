using System.Collections.Generic;
using GtMotive.Estimate.Microservice.Domain.Models;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Represents a use case for obtaining the list of vehicles available for renting.
    /// </summary>
    public interface IGetAvailableVehiclesUseCase
    {
        /// <summary>
        /// Executes the retrieval of all available vehicles.
        /// </summary>
        /// <returns>The result containing the list of available vehicles.</returns>
        IEnumerable<Vehicle> Execute();
    }
}
