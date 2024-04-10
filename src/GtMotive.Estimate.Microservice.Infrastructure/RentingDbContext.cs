using System;
using System.Collections.ObjectModel;
using GtMotive.Estimate.Microservice.Domain.Models;

namespace GtMotive.Estimate.Microservice.Infrastructure
{
    /// <summary>
    /// Represents the database context for the application.
    /// </summary>
    public class RentingDbContext
    {
        private static readonly object _lock = new();
        private static RentingDbContext _instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentingDbContext"/> class.
        /// Constructor.
        /// </summary>
        public RentingDbContext()
        {
            Vehicles = new Collection<Vehicle>();
            Rentals = new Collection<Rental>();

            // Initialize example data
            InitializeData();
        }

        /// <summary>
        /// Gets the singleton instance of RentingDbContext.
        /// </summary>
        public static RentingDbContext Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new RentingDbContext();
                }
            }
        }

        /// <summary>
        /// Gets vehicles.
        /// </summary>
        public Collection<Vehicle> Vehicles { get; }

        /// <summary>
        /// Gets vehicles.
        /// </summary>
        public Collection<Rental> Rentals { get; }

        /// <summary>
        /// Initialize example data.
        /// </summary>
        private void InitializeData()
        {
            // Examples of real brands and models
            string[] brands = { "Toyota", "Ford", "Chevrolet", "Honda", "Nissan", "Volkswagen", "BMW", "Audi", "Mercedes-Benz", "Hyundai" };
            string[] models = { "Corolla", "F-150", "Camaro", "Civic", "Altima", "Jetta", "3 Series", "A4", "C-Class", "Elantra" };

            // Add 10 examples of vehicles with brands and models
            for (var i = 0; i < 10; i++)
            {
                var vehicle = new Vehicle
                {
                    Id = i + 1,
                    Brand = brands[i],
                    Model = models[i],
                    ManufacturingDate = DateTime.UtcNow.AddYears(-(i % 5)).AddDays(i % 5).AddMonths(i).AddHours(i).AddMinutes(i % 5).AddSeconds(i) // Random manufacturing date
                };

                Vehicles.Add(vehicle);
            }

            // Add two rentals that have not been returned
            Rentals.Add(new Rental { Id = 1, VehicleId = 6, RenterId = 1, RentalDate = DateTime.UtcNow.AddDays(-7), ReturnDate = null });
            Rentals.Add(new Rental { Id = 2, VehicleId = 8, RenterId = 2, RentalDate = DateTime.UtcNow.AddDays(-3), ReturnDate = null });

            // Add two rentals that have already been returned
            Rentals.Add(new Rental { Id = 3, VehicleId = 8, RenterId = 1, RentalDate = DateTime.UtcNow.AddDays(-3), ReturnDate = DateTime.UtcNow.AddHours(-1) });
            Rentals.Add(new Rental { Id = 4, VehicleId = 8, RenterId = 2, RentalDate = DateTime.UtcNow.AddDays(-3), ReturnDate = DateTime.UtcNow.AddHours(-2) });
        }
    }
}
