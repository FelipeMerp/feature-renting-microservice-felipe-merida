using System.Collections.ObjectModel;
using GtMotive.Estimate.Microservice.Domain.Models;

namespace GtMotive.Estimate.Microservice.Infrastructure.Interfaces
{
    /// <summary>
    /// Represents the database context for the application.
    /// </summary>
    public interface IRentingDbContext
    {
        /// <summary>
        /// Gets vehicles.
        /// </summary>
        Collection<Vehicle> Vehicles { get; }

        /// <summary>
        /// Gets rentals.
        /// </summary>
        Collection<Rental> Rentals { get; }
    }
}
