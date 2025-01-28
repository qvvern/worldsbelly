//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using PuppeteerSharp;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Threading.Tasks;
//using WorldsBelly.API.Models;
//using WorldsBelly.API.Services.Interfaces;
//using WorldsBelly.DataAccess.Contexts;
//using WorldsBelly.DataAccess.Utilities.Exceptions;
//using WorldsBelly.Domain.Utils.Helpers;

//namespace WorldsBelly.API.Controllers
//{
//    [Produces("application/json")]
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UserTagsController : ControllerBase
//    {
//        private readonly IUserTagService _service;

//        public UserTagsController(IUserTagService service)
//        {
//            _service = service;
//        }

//        /// <summary>
//        /// Get UserTags
//        /// </summary>
//        /// <remarks>
//        /// Sample request:
//        ///
//        ///     GET api/userTags
//        ///
//        /// </remarks>
//        /// <response code="200">Returns userTags</response>
//        /// <response code="404">If the userTags is not found</response>
//        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
//        [HttpGet, AllowAnonymous]
//        [ProducesResponseType(typeof(ICollection<UserTagView>), StatusCodes.Status200OK)]
//        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
//        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<ICollection<UserTagView>>> GetUserTags(
//            [FromQuery] int? startAt,
//            [FromQuery] int? amount,
//            [FromQuery] string search)
//        {
//            try
//            {
//                return await _service.GetUserTagsAsync(startAt, amount, search);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e);
//            }
//        }



//        /// <summary>
//        /// Create Tags
//        /// </summary>
//        /// <remarks>
//        /// Sample request:
//        ///
//        ///     POST api/UserTags
//        ///
//        /// </remarks>
//        /// <response code="200">Returns OK if tag was created</response>
//        /// <response code="400">If tag request is invalid</response>
//        /// <response code="401">If the authentication token is not provided or is expired - Format: Bearer JWT</response>
//        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
//        [HttpPost]
//        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
//        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<ICollection<UserTagView>>> CalculateStatistics([FromBody, BindRequired] ICollection<UserTagView> tags)
//        {
//            try
//            {
//                return Ok(await _service.CreateTagsAsync(tags));
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e);
//            }
//        }
//    }
//}