using System;
using System.ComponentModel.DataAnnotations;

namespace GtMotive.Estimate.Microservice.Domain.Models
{
    /// <summary>
    /// Represents a vehicle available for rental.
    /// </summary>
    public class Vehicle
    {
        /// <summary>
        /// Gets or sets unique identifier of the vehicle.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets brand of the vehicle (e.g., Toyota, Ford, etc.).
        /// </summary>
        [Required(ErrorMessage = "brand is required for vehicle.")]
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets model of the vehicle (e.g., Corolla, Mustang, etc.).
        /// </summary>
        [Required(ErrorMessage = "model is required for vehicle.")]
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets manufacturing date of the vehicle.
        /// </summary>
        [Required(ErrorMessage = "manufacturingDate is required.")]
        public DateTime? ManufacturingDate { get; set; }
    }
}
