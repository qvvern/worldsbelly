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
using WorldsBelly.DataAccess.Utilities.Extensions;
using WorldsBelly.Domain.Utils.Helpers;

namespace WorldsBelly.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesController : ControllerBase
    {
        private readonly ILanguageService _service;

        public LanguagesController(ILanguageService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get Languages
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/countries
        ///
        /// </remarks>
        /// <response code="200">Returns countries</response>
        /// <response code="404">If the countries is not found</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpGet, AllowAnonymous]
        [ProducesResponseType(typeof(ICollection<LanguageView>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, VaryByHeader = "Language-Id")]
        public async Task<ActionResult<ICollection<LanguageView>>> GetLanguages()
        {
            //int languageId = Request.Headers.GetLanguageId();

            try
            {
                return await _service.GetLanguagesAsync();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}