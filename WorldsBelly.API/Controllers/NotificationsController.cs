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
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationsController(INotificationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get Notifications by signed in user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/notifications
        ///
        /// </remarks>
        /// <response code="200">Returns notifications</response>
        /// <response code="404">If the countries is not found</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<NotificationView>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ResponseCache(NoStore = true, Duration = 0)]
        public async Task<ActionResult<ICollection<NotificationView>>> GetNotifications(
            [FromQuery] int? startAt,
            [FromQuery] int? amount)
        {
            try
            {
                return await _service.GetNotificationsAsync(startAt, amount);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Get Notification by signed in user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/notifications/id
        ///
        /// </remarks>
        /// <param name="id">id of recipe</param>
        /// <response code="200">Returns notifications</response>
        /// <response code="404">If the countries is not found</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NotificationView), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ResponseCache(NoStore = true, Duration = 0)]
        public async Task<ActionResult<NotificationView>> GetNotification([FromRoute, BindRequired] int id)
        {
            try
            {
                return await _service.GetNotificationAsync(id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Delete Notifications
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE api/notifications/
        ///
        /// </remarks>
        /// <response code="204">No Content</response>
        /// <response code="400">If ingredient request is invalid</response>
        /// <response code="401">If the authentication token is not provided or is expired - Format: Bearer JWT</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteNotification()
        {
            try
            {
                await _service.DeleteNotificationsAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Read Notification
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     UPDATE api/notifications/
        ///
        /// </remarks>
        /// <param name="id">id of recipe</param>
        /// <response code="204">No Content</response>
        /// <response code="400">If ingredient request is invalid</response>
        /// <response code="401">If the authentication token is not provided or is expired - Format: Bearer JWT</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ReadNotificationAsync([FromRoute, BindRequired] int id)
        {
            try
            {
                await _service.ReadNotificationAsync(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Read Notifications
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     UPDATE api/notifications/
        ///
        /// </remarks>
        /// <response code="204">No Content</response>
        /// <response code="400">If ingredient request is invalid</response>
        /// <response code="401">If the authentication token is not provided or is expired - Format: Bearer JWT</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ReadNotificationsAsync()
        {
            try
            {
                await _service.ReadNotificationsAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}