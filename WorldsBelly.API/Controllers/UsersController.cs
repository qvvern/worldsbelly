using Autofac.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;
using WorldsBelly.API.Models;
using WorldsBelly.API.Services.Interfaces;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Models;
using WorldsBelly.DataAccess.Repository.Interfaces;
using WorldsBelly.DataAccess.Services.Interfaces;
using WorldsBelly.DataAccess.Utilities.Exceptions;
using WorldsBelly.DataAccess.Utilities.Extensions;

namespace WorldsBelly.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IHeaderService _headerService;
        private readonly IAzureAdB2CService _azureAdB2CService;
        private readonly IUserRepository _userRepository;
        private readonly IRecipeService _service;

        public UsersController(IHeaderService headerService, IUserRepository userRepos, IAzureAdB2CService azureAdB2CService, IRecipeService service)
        {
            _headerService = headerService;
            _userRepository = userRepos;
            _azureAdB2CService = azureAdB2CService;
            _service = service;
        }

        /// <summary>
        /// Get recipes related to user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/user/recipes
        ///
        /// </remarks>
        /// <param name="filter">Filter options</param>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpPost("Recipes"), AllowAnonymous]
        [ProducesResponseType(typeof(RecipeCollectionResponse<dynamic>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RecipeCollectionResponse<RecipeView>>> GetRecipesAsync(
            [FromBody, BindRequired] RecipeRelatedToUserFilterQuery filter)
        {
            try
            {
                return await _service.GetRecipesRelatedToUserAsync(filter);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Get Signed In User
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/SignedInUser
        ///
        /// </remarks>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpGet("SignedInUser")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> GetSignedInUserAsync()
        {
            try
            {
                Guid signedInUserId = _headerService.GetUserId();
                User response = await _userRepository.GetUserByAzureIdAsync(signedInUserId);
                if (response == null)
                {
                    throw new Exception("Could not find user");
                }
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// GetOrCreate Signed In User
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/SignedInUser
        ///
        /// </remarks>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpPost("SignedInUser")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> GetOrCreateSignedInUserAsync()
        {
            try
            {
                Guid signedInUserId = _headerService.GetUserId();
                User response = await _userRepository.GetUserByAzureIdAsync(signedInUserId);
                if (response == null)
                {
                    response = await _userRepository.CreateUser(signedInUserId);
                }
                if (response == null)
                {
                    throw new Exception("Could not find user");
                }
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Verify Username
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/User/username/{username}
        ///
        /// </remarks>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        /// <param name="username">id of user</param>
        [HttpGet("username/{username}")]
        [ProducesResponseType(typeof(ActionResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> VerifyUsernameAsync(
            [FromRoute, BindRequired] string username)
        {
            try
            {
                return Ok(await _userRepository.IsUsernameVerifiedAsync(username));
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Get User by username
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/user/{username}
        ///
        /// </remarks>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        /// <param name="username">id of user</param>
        [HttpGet("user/{username}"), AllowAnonymous]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> GetUserAsync([FromRoute, BindRequired] string username)
        {
            try
            {
                User response = await _userRepository.GetUserByUsernameAsync(username);
                if (response == null)
                {
                    throw new Exception("Could not find user");
                }
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
        /// <summary>
        /// Update Signed In Username
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     UPDATE api/SignedInUser/{username}
        ///
        /// </remarks>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        /// <param name="username">username of user</param>
        [HttpPut("SignedInUser/{username}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateUserAsync([FromRoute, BindRequired] string username)
        {
            try
            {
                Guid signedInUserId = _headerService.GetUserId();
                User existingUser = await _userRepository.GetUserByAzureIdAsync(signedInUserId);
                if (existingUser == null)
                {
                    throw new Exception("Could not find user");
                }

                existingUser.Username = username.Trim();

                await _userRepository.UpdateSignedInUserAsync(existingUser);
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }



        /// <summary>
        /// Update Signed In User
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     UPDATE api/SignedInUser
        ///
        /// </remarks>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpPut("SignedInUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateUserAsync([FromBody, BindRequired] User user)
        {
            try
            {
                Guid signedInUserId = _headerService.GetUserId();
                User existingUser = await _userRepository.GetUserByAzureIdAsync(signedInUserId);
                if (existingUser == null)
                {
                    throw new Exception("Could not find user");
                }

                await _userRepository.UpdateSignedInUserAsync(user);
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }


        /// <summary>
        /// Update SignedInMsalUser
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     UPDATE api/users/SignedInMsalUser
        ///
        /// </remarks>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpPut("SignedInMsalUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateUserAsync([FromBody, BindRequired] AzureAdB2CUser user)
        {
            try
            {
                Guid signedInUserId = _headerService.GetUserId();
                AzureAdB2CUser userResponse = await _azureAdB2CService.GetUserAsync(signedInUserId.ToString());

                if (userResponse.Id == signedInUserId)
                {
                    if(!String.IsNullOrWhiteSpace(user.Mail) && user.Mail != userResponse.Mail && userResponse.Mail != userResponse.VerificationNewEmail && userResponse.VerificationNewEmail == user.Mail)
                    {
                        var userUpdateEmail = new UserUpdateEmail()
                        {
                            NewEmail = userResponse.VerificationNewEmail,
                            OldEmail = userResponse.Mail,
                            VerificationCode = user.VerificationCode
                        };
                        DateTime currentTime = DateTime.Now;
                        if (userResponse.Id != signedInUserId || String.IsNullOrEmpty(userResponse.VerificationCode))
                        {
                            return NoContent();
                        }
                        if (String.IsNullOrWhiteSpace(userResponse.VerificationCodeExpiration) || currentTime > DateTime.Parse(userResponse.VerificationCodeExpiration))
                        {
                            await _azureAdB2CService.ResetUserVerificationCode(signedInUserId.ToString());
                            throw new Exception("Code expired.");
                        }
                        if (userUpdateEmail.VerificationCode != userResponse.VerificationCode || String.IsNullOrWhiteSpace(userResponse.VerificationCode) || String.IsNullOrWhiteSpace(userUpdateEmail.VerificationCode))
                        {
                            throw new Exception("Code is invalid.");
                        }
                        if (userUpdateEmail.OldEmail != userResponse.Mail || String.IsNullOrWhiteSpace(userUpdateEmail.OldEmail))
                        {
                            throw new Exception("email doesnt match.");
                        }
                        if (userUpdateEmail.NewEmail != userResponse.VerificationNewEmail || String.IsNullOrWhiteSpace(userResponse.VerificationNewEmail))
                        {
                            throw new Exception("new email is not valid.");
                        }
                        await _azureAdB2CService.UpdateUserEmail(signedInUserId.ToString(), userUpdateEmail.NewEmail);
                        await _azureAdB2CService.ResetUserVerificationCode(signedInUserId.ToString());
                    }
                    if(user.GivenName != userResponse.GivenName || user.Surname != userResponse.Surname)
                    {
                        await _azureAdB2CService.UpdateUserAsync(user, signedInUserId);
                    }
                }

                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Submit users new email for verification and update
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     UPDATE api/users/email/submit
        ///
        /// </remarks>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpPut("email/submit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateUserEmailSubmitAsync([FromBody, BindRequired] UserUpdateEmail userUpdateEmail)
        {
            try
            {

                Guid signedInUserId = _headerService.GetUserId();
                AzureAdB2CUser userResponse = await _azureAdB2CService.GetUserAsync(signedInUserId.ToString());

                DateTime currentTime = DateTime.Now;
                if (!String.IsNullOrWhiteSpace(userResponse.VerificationCodeExpiration) && currentTime < DateTime.Parse(userResponse.VerificationCodeExpiration))
                {
                    TimeSpan ts = DateTime.Parse(userResponse.VerificationCodeExpiration) - currentTime;
                    throw new Exception($"Email verification already sent to {userResponse.Mail}. Please check your spam folder. Expires in " + Math.Round(ts.TotalMinutes) + " minutes.");
                }

                if (userResponse.Id == signedInUserId && userUpdateEmail.OldEmail == userResponse.Mail && !String.IsNullOrWhiteSpace(userUpdateEmail.NewEmail))
                {
                    if(IsValidEmail(userUpdateEmail.NewEmail))
                    {
                        var foundUserEmail = await _azureAdB2CService.DoesEmailAlreadyExistAsync(userUpdateEmail.NewEmail.Trim());
                        await _azureAdB2CService.UpdateUserVerificationCode(signedInUserId.ToString(), userResponse.Mail, userUpdateEmail.NewEmail.Trim(), _headerService.GetToken());
                    }
                    else
                    {
                        throw new Exception("email is invalid");
                    }
                }
                else
                {
                    throw new Exception("wrong inputs");
                }

                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Submit delete profile for verification and update
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     UPDATE api/users/delete/submit
        ///
        /// </remarks>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        [HttpPut("delete/submit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteProfileSubmitAsync()
        {
            try
            {

                Guid signedInUserId = _headerService.GetUserId();
                AzureAdB2CUser userResponse = await _azureAdB2CService.GetUserAsync(signedInUserId.ToString());

                DateTime currentTime = DateTime.Now;
                if (!String.IsNullOrWhiteSpace(userResponse.DeleteUserVerificationCodeExpiration) && currentTime < DateTime.Parse(userResponse.DeleteUserVerificationCodeExpiration))
                {
                    TimeSpan ts = DateTime.Parse(userResponse.DeleteUserVerificationCodeExpiration) - currentTime;
                    throw new Exception($"Email verification already sent to {userResponse.Mail}. Please check your spam folder. Expires in " + Math.Round(ts.TotalMinutes) + " minutes.");
                }

                if (userResponse.Id == signedInUserId)
                {
                    await _azureAdB2CService.UpdateDeleteUserVerificationCode(signedInUserId.ToString(), userResponse.Mail, _headerService.GetToken());
                }
                else
                {
                    throw new Exception("wrong inputs");
                }

                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }


        /// <summary>
        /// Delete SignedInUser
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     UPDATE api/users/delete/{code}
        ///
        /// </remarks>
        /// <response code="200">Returns a string that represents that URLs source code</response>
        /// <response code="400">If the url is invalid and the validation criteria are not met</response>
        /// <response code="500">In case of errors occurring on the data layer or anywhere on the server's side</response>
        /// <param name="verificationCode">id of user</param>
        [HttpDelete("Delete/{verificationCode}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteUserAsync([FromRoute, BindRequired] string verificationCode)
        {
            try
            {
                string code = System.Uri.UnescapeDataString(verificationCode);
                Guid signedInUserId = _headerService.GetUserId();
                AzureAdB2CUser userResponse = await _azureAdB2CService.GetUserAsync(signedInUserId.ToString());

                if (userResponse.Id == signedInUserId)
                {
                    DateTime currentTime = DateTime.Now;
                    if (userResponse.Id != signedInUserId || String.IsNullOrEmpty(userResponse.DeleteUserVerificationCode))
                    {
                        return NoContent();
                    }
                    if (String.IsNullOrWhiteSpace(userResponse.DeleteUserVerificationCodeExpiration) || currentTime > DateTime.Parse(userResponse.DeleteUserVerificationCodeExpiration))
                    {
                        await _azureAdB2CService.ResetUserVerificationCode(signedInUserId.ToString());
                        throw new Exception("Code expired.");
                    }
                    if (code != userResponse.DeleteUserVerificationCode || String.IsNullOrWhiteSpace(userResponse.DeleteUserVerificationCode) || String.IsNullOrWhiteSpace(code))
                    {
                        throw new Exception("Code is invalid.");
                    }
                    try
                    {
                        await _userRepository.DeleteUserAsync(signedInUserId);
                        await _azureAdB2CService.DeleteUser(signedInUserId.ToString());
                    }
                    catch
                    {
                        throw new Exception("Sorry, something went wrong when trying to delete your user. Try again or contact support.");
                    }
                }

                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }
}