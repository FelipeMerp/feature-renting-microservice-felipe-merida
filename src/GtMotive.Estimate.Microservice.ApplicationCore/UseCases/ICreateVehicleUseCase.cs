using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Models;
using Microsoft.AspNetCore.Mvc;

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
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<ActionResult<string>> Execute(Vehicle vehicle);
    }
}
