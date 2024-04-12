using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Models;
using Microsoft.AspNetCore.Mvc;

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
        Task<ActionResult<IEnumerable<Vehicle>>> Execute();
    }
}
