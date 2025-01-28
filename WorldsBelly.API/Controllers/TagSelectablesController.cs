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
    public class TagSelectablesController : ControllerBase
    {
        private readonly ITagSelectableService _service;

        public TagSelectablesController(ITagSelectableService service)
        {
            _service = service;
        }


        /// <summary>
        /// Get TagSelectableChoiceOrders
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/tagSelectables/ChoiceOrders
        ///
        /// </remarks>
        /// <response code="200">Returns tagSelectableChoiceOrders</response>
        /// <response code="401">If the authentication token is not provided or is expired - Format: Bearer JWT</response>
        /// <response code="404">If the tagSelectableChoiceOrders is not found</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpGet("ChoiceOrders")]
        [ProducesResponseType(typeof(ICollection<TagSelectableChoiceOrderView>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, VaryByHeader = "Language-Id")]
        public async Task<ActionResult<ICollection<TagSelectableChoiceOrderView>>> GetTagSelectableChoiceOrders()
        {
            try
            {
                return await _service.GetTagSelectableChoiceOrdersAsync();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

    }
}