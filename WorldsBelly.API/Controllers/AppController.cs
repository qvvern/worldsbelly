using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.API.Models;
using WorldsBelly.API.Services.Interfaces;
using WorldsBelly.DataAccess.Contexts;
using WorldsBelly.DataAccess.Services.Interfaces;
using WorldsBelly.DataAccess.Utilities.Exceptions;
using WorldsBelly.DataAccess.Utilities.Extensions;
using WorldsBelly.Domain.Utils.Helpers;

namespace WorldsBelly.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AppsController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly INutrientService _nutrientService;
        private readonly IMeasurementService _measurementService;
        private readonly IRecipeService _recipeService;

        public AppsController(ICountryService countryService, INutrientService nutrientService, IMeasurementService measurementService, IRecipeService recipeService)
        {
            _countryService = countryService;
            _nutrientService = nutrientService;
            _measurementService = measurementService;
            _recipeService = recipeService;
        }

        /// <summary>
        /// Get App Configurations
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/Apps
        ///
        /// </remarks>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpGet("configs"), AllowAnonymous]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "*" }, VaryByHeader = "Language-Id")]
        public async Task<ActionResult<object>> GetAppsAsync()
        {
            int languageId = Request.Headers.GetLanguageId();
            try
            {
                ActionResult<ICollection<CountryView>> countries = await _countryService.GetCountriesAsync(languageId);
                ActionResult<ICollection<NutrientView>> nutrients = await _nutrientService.GetNutrientsAsync();
                ActionResult<ICollection<MeasurementView>> measurements = await _measurementService.GetMeasurementsAsync();
                ActionResult<ICollection<RecipeBestServedView>> recipeBestServed = await _recipeService.GetRecipeBestServedAsync();
                ActionResult<ICollection<RecipeConsumerView>> recipeConsumer = await _recipeService.GetRecipeConsumerAsync();
                ActionResult<ICollection<RecipeAgeGroupView>> recipeAgeGroup = await _recipeService.GetRecipeAgeGroupAsync();
                ActionResult<ICollection<RecipeDifficultyView>> recipeDifficulty = await _recipeService.GetRecipeDifficultyAsync();

                dynamic result = new ExpandoObject();
                result.countries = countries.Value;
                result.nutrients = nutrients.Value;
                result.measurements = measurements.Value;
                result.recipeBestServed = recipeBestServed.Value;
                result.recipeConsumer = recipeConsumer.Value;
                result.recipeAgeGroup = recipeAgeGroup.Value;
                result.recipeDifficulty = recipeDifficulty.Value;
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}