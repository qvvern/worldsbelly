using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WorldsBelly.API.Models;
using WorldsBelly.API.Services.Interfaces;
using WorldsBelly.DataAccess.Contexts;
using WorldsBelly.DataAccess.Utilities.Exceptions;
using WorldsBelly.Domain.Utils.Helpers;

namespace WorldsBelly.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientService _service;

        public IngredientsController(IIngredientService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get Ingredients
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/ingredients
        ///
        /// </remarks>
        /// <response code="200">Returns ingredients</response>
        /// <response code="404">If the ingredients is not found</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpGet, AllowAnonymous]
        [ProducesResponseType(typeof(ICollection<IngredientView>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ResponseCache(NoStore = true, Duration = 0)]
        public async Task<ActionResult<ICollection<IngredientView>>> GetIngredients(
            [FromQuery] int? startAt,
            [FromQuery] int? amount,
            [FromQuery] string search)
        {
            try
            {
                return await _service.GetIngredientsAsync(startAt, amount, search);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Get ingredients by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/ingredients/id
        ///
        /// </remarks>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpPost("id"), AllowAnonymous]
        [ProducesResponseType(typeof(ICollection<IngredientView>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ICollection<IngredientView>>> GetIngredientsById(
            [FromBody, BindRequired] List<int> ids)
        {
            try
            {
                return await _service.GetIngredientsByIdAsync(ids);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}