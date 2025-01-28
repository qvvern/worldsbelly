using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.API.Models;
using WorldsBelly.API.Services.Interfaces;
using WorldsBelly.DataAccess.Contexts;
using WorldsBelly.DataAccess.Services.Interfaces;
using WorldsBelly.DataAccess.Utilities.Exceptions;
using WorldsBelly.Domain.Utils.Helpers;

namespace WorldsBelly.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeDraftsController : ControllerBase
    {
        private readonly IRecipeDraftService _service;

        public RecipeDraftsController(IRecipeDraftService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get Recipe Drafts
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/RecipeDrafts
        ///
        /// </remarks>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ICollection<RecipeView>>> GetRecipeDraftsAsync()
        {
            try
            {
                string currentUserId = User.Identity.GetUserId();
                return await _service.GetRecipeDraftsAsync();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Get Recipe Draft
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/RecipeDrafts/{translationId}
        ///
        /// </remarks>
        /// <param name="id">id of recipe draft translation</param>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RecipeView>> GetRecipeDraftAsync(
            [FromRoute, BindRequired] Guid id)
        {
            try
            {
                return await _service.GetRecipeDraftAsync(id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Update Recipe Draft
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     UPDATE api/RecipeDrafts/{translationId}
        ///
        /// </remarks>
        /// <param name="id">id of recipe draft translation</param>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RecipeView>> UpdateRecipeDraftAsync(
            [FromRoute, BindRequired] Guid id, [FromBody, BindRequired] RecipeView recipeDraft)
        {
            try
            {
                return await _service.UpdateRecipeDraftAsync(id, recipeDraft);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Publish Recipe Draft
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     UPDATE api/RecipeDrafts/{translationId}
        ///
        /// </remarks>
        /// <param name="id">id of recipe draft translation</param>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpPut("publish/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PublishRecipeDraftAsync(
            [FromRoute, BindRequired] Guid id, [FromBody, BindRequired] RecipeView recipeDraft)
        {
            try
            {
                await _service.UpdateRecipeDraftAsync(id, recipeDraft);
                await _service.PublishRecipeDraftAsync(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Create Recipe Draft
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/RecipeDrafts/create
        ///
        /// </remarks>
        /// <response code="200">Returns OK if ingredient was created</response>
        /// <response code="400">If ingredient request is invalid</response>
        /// <response code="401">If the authentication token is not provided or is expired - Format: Bearer JWT</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RecipeView>> CreateRecipeDraftAsync([FromBody, BindRequired] CreateRecipeDraftView recipeDraft)
        {
            try
            {
                return await _service.CreateRecipeDraftTranslationAsync(recipeDraft);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Delete Recipe Draft
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE api/RecipeDrafts/{translationId}
        ///
        /// </remarks>
        /// <param name="id">id of translation</param>
        /// <response code="200">Returns OK if ingredient was created</response>
        /// <response code="400">If ingredient request is invalid</response>
        /// <response code="401">If the authentication token is not provided or is expired - Format: Bearer JWT</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteRecipeDraftAsync([FromRoute, BindRequired] Guid id)
        {
            try
            {
                return await _service.DeleteRecipeDraftTranslationAsync(id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}