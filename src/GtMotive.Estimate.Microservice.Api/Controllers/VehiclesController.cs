﻿using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    /// <summary>
    /// Controller to handle operations related to vehicles.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly ICreateVehicleUseCase _createVehicleUseCase;
        private readonly IGetAvailableVehiclesUseCase _getAvailableVehiclesUseCase;
        private readonly IGetVehicleUseCase _getVehicleUseCase;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehiclesController"/> class.
        /// Constructor for the VehiclesController class.
        /// </summary>
        /// <param name="createVehicleUseCase">An instance of the create vehicle use case.</param>
        /// <param name="getAvailableVehiclesUseCase">An instance of the get available vehicles use case.</param>
        /// <param name="getVehicleUseCase">An instance of the get vehicle use case.</param>
        public VehiclesController(ICreateVehicleUseCase createVehicleUseCase, IGetAvailableVehiclesUseCase getAvailableVehiclesUseCase, IGetVehicleUseCase getVehicleUseCase)
        {
            _createVehicleUseCase = createVehicleUseCase;
            _getAvailableVehiclesUseCase = getAvailableVehiclesUseCase;
            _getVehicleUseCase = getVehicleUseCase;
        }

        /// <summary>
        /// HTTP POST method to create a new vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle object to be created.</param>
        /// <returns>The result of the vehicle creation operation.</returns>
        [HttpPost("createVehicle")]
        public Task<ActionResult<string>> CreateVehicle([FromBody] Vehicle vehicle)
        {
            return _createVehicleUseCase.Execute(vehicle);
        }

        /// <summary>
        /// HTTP GET method to retrieve all available vehicles.
        /// </summary>
        /// <returns>The result containing the list of available vehicles.</returns>
        [HttpGet("availableVehicles")]
        public Task<ActionResult<IEnumerable<Vehicle>>> GetAvailableVehicles()
        {
            return _getAvailableVehiclesUseCase.Execute();
        }

        /// <summary>
        /// HTTP GET method to retrieve a vehicle by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to retrieve.</param>
        /// <returns>The result containing the retrieved vehicle.</returns>
        [HttpGet("getVehicleById/{id}")]
        public ActionResult<string> GetVehicle(int id)
        {
            return _getVehicleUseCase.Execute(id);
        }
    }
}
