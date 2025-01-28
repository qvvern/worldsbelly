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
    public class NutrientsController : ControllerBase
    {
        private readonly INutrientService _service;

        public NutrientsController(INutrientService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get Nutrients
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/nutrients
        ///
        /// </remarks>
        /// <response code="200">Returns nutrients</response>
        /// <response code="404">If the nutrients is not found</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpGet, AllowAnonymous]
        [ProducesResponseType(typeof(ICollection<NutrientView>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, VaryByHeader = "Language-Id")]
        public async Task<ActionResult<ICollection<NutrientView>>> GetNutrients()
        {
            try
            {
                return await _service.GetNutrientsAsync();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}