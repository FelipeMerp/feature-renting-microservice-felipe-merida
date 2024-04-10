using System;
using System.ComponentModel.DataAnnotations;

namespace GtMotive.Estimate.Microservice.Domain.Models
{
    /// <summary>
    /// Represents a rental of a vehicle.
    /// </summary>
    public class Rental
    {
        /// <summary>
        /// Gets or sets unique identifier of the rental.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets identifier of the vehicle associated with the rental.
        /// </summary>
        [Required(ErrorMessage = "Vehicle Id is required for rental.")]
        public int VehicleId { get; set; }

        /// <summary>
        /// Gets or sets identifier of the renter (customer) who rented the vehicle.
        /// </summary>
        [Required(ErrorMessage = "Renter Id is required for rental.")]
        public int RenterId { get; set; }

        /// <summary>
        /// Gets or sets date when the vehicle was rented.
        /// </summary>
        public DateTime RentalDate { get; set; }

        /// <summary>
        /// Gets or sets date when the vehicle was returned (can be null if not yet returned).
        /// </summary>
        public DateTime? ReturnDate { get; set; }
    }
}
