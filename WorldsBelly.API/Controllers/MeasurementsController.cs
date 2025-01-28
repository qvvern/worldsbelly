using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorldsBelly.API.Models;
using WorldsBelly.API.Services.Interfaces;
using WorldsBelly.DataAccess.Utilities.Exceptions;

namespace WorldsBelly.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementsController : ControllerBase
    {
        private readonly IMeasurementService _service;

        public MeasurementsController(IMeasurementService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get Measurements
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/measurements
        ///
        /// </remarks>
        /// <response code="200">Returns measurements</response>
        /// <response code="404">If the measurements is not found</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpGet, AllowAnonymous]
        [ProducesResponseType(typeof(ICollection<MeasurementView>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, VaryByHeader = "Language-Id")]
        public async Task<ActionResult<ICollection<MeasurementView>>> GetMeasurements()
        {
            try
            {
                return await _service.GetMeasurementsAsync();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}