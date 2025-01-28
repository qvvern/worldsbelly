using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WorldsBelly.API.Models;
using WorldsBelly.API.Services.Interfaces;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Models;
using WorldsBelly.DataAccess.Utilities.Exceptions;

namespace WorldsBelly.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _service;

        public RecipesController(IRecipeService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get recipes
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/recipes
        ///
        /// </remarks>
        /// <param name="filter">Filter options</param>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpPost, AllowAnonymous]
        [ProducesResponseType(typeof(RecipeCollectionResponse<dynamic>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RecipeCollectionResponse<RecipeView>>> GetRecipesAsync(
            [FromBody, BindRequired] RecipeFilterQuery filter)
        {
            try
            {
                return await _service.GetRecipesAsync(filter);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Get Recipe
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/Recipe/{id}
        ///
        /// </remarks>
        /// <param name="id">id of recipe</param>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpGet("{id}"), AllowAnonymous]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RecipeView>> GetRecipeAsync([FromRoute, BindRequired] Guid id)
        {
            try
            {
                return await _service.GetRecipeAsync(id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Get Recipe Steps
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/Recipe/steps/{id}
        ///
        /// </remarks>
        /// <param name="id">id of recipe</param>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpGet("steps/{id}"), AllowAnonymous]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "*" }, VaryByHeader = "Language-Id")]
        public async Task<ActionResult<ICollection<RecipeStepView>>> GetRecipeStepsAsync([FromRoute, BindRequired] Guid id)
        {
            try
            {
                return await _service.GetRecipeStepsAsync(id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Get BestServed
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/Recipes/BestServed
        ///
        /// </remarks>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpGet("BestServed"), AllowAnonymous]
        [ProducesResponseType(typeof(ICollection<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "*" }, VaryByHeader = "Language-Id")]
        public async Task<ActionResult<ICollection<RecipeBestServedView>>> GetRecipeBestServedAsync()
        {
            try
            {
                return await _service.GetRecipeBestServedAsync();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Get Consumer
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/Recipes/Consumer
        ///
        /// </remarks>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpGet("Consumer"), AllowAnonymous]
        [ProducesResponseType(typeof(ICollection<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "*" }, VaryByHeader = "Language-Id")]
        public async Task<ActionResult<ICollection<RecipeConsumerView>>> GetRecipeConsumerAsync()
        {
            try
            {
                return await _service.GetRecipeConsumerAsync();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Get AgeGroup
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/Recipes/AgeGroup
        ///
        /// </remarks>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpGet("AgeGroup"), AllowAnonymous]
        [ProducesResponseType(typeof(ICollection<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "*" }, VaryByHeader = "Language-Id")]
        public async Task<ActionResult<ICollection<RecipeAgeGroupView>>> GetRecipeAgeGroupAsync()
        {
            try
            {
                return await _service.GetRecipeAgeGroupAsync();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Get Difficulty
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/Recipes/Difficulty
        ///
        /// </remarks>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpGet("Difficulty"), AllowAnonymous]
        [ProducesResponseType(typeof(ICollection<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "*" }, VaryByHeader = "Language-Id")]
        public async Task<ActionResult<ICollection<RecipeDifficultyView>>> GetRecipeDifficultyAsync()
        {
            try
            {
                return await _service.GetRecipeDifficultyAsync();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Get recipe user rating
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/Recipes/id/user/rating
        ///
        /// </remarks>
        /// <param name="id">id of recipe</param>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpGet("{id}/user/rating")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RecipeRatingView>> GetRecipeUserRatingAsync(
            [FromRoute, BindRequired] int id)
        {
            try
            {
                return await _service.GetRecipeUserRatingAsync(id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Submit recipe user rating
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POAT api/Recipes/user/rating
        ///
        /// </remarks>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpPost("user/rating")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RecipeRatingView>> SubmitRecipeUserRatingAsync(
            [FromBody, BindRequired] RecipeRatingView rating)
        {
            try
            {
                return await _service.SubmitRecipeUserRatingAsync(rating);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Like/Unlike recipe
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/Recipes/{id}/like
        ///
        /// </remarks>
        /// <param name="id">id of recipe</param>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpPost("{id}/like")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> LikeRecipeAsync(
            [FromRoute, BindRequired] int id)
        {
            try
            {
                await _service.LikeRecipeAsync(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Get Like/Unlike recipe
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/Recipes/{id}/like
        ///
        /// </remarks>
        /// <param name="id">id of recipe</param>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpGet("{id}/like")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> GetLikeRecipeAsync(
            [FromRoute, BindRequired] int id)
        {
            try
            {
                return await _service.GetLikeRecipeAsync(id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}