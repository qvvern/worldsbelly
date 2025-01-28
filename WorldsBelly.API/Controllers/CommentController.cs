using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.API.Models;
using WorldsBelly.API.Requests;
using WorldsBelly.API.Responses;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Repository.Interfaces;
using WorldsBelly.DataAccess.Utilities;
using WorldsBelly.DataAccess.Utilities.Exceptions;
using WorldsBelly.DataAccess.Utilities.Extensions;

namespace WorldsBelly.API.Controllers
{
    [AllowAnonymous]
    [Produces("application/json")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private ICommentRepository _commentRepos;
        private IRecipeRepository _recipeRepos;
        private IUserRepository _userRepos;
        public CommentController(
            IRecipeRepository recipeRepos,
            ICommentRepository commentRepos,
            IUserRepository userRepos)
        {
            _recipeRepos = recipeRepos;
            _commentRepos = commentRepos;
            _userRepos = userRepos;
        }

        /// <param name="recipeId">The recipe to get comments for</param>
        /// <param name="orderBy">DateCreatedAsc, DateCreatedDesc</param>
        [HttpGet]
        [Route("api/recipes/{recipeId}/comments/")]
        public async Task<ActionResult<ICollection<CommentResponse>>> GetRecipeComments(int recipeId, string orderBy = null)
        {
            try
            {
                int languageId = Request.Headers.GetLanguageId();

                QueryOrderOptions orderOptions = QueryOrderOptions.DateCreatedAsc;
                if (!string.IsNullOrWhiteSpace(orderBy))
                {
                    orderOptions = (QueryOrderOptions)Enum.Parse(typeof(QueryOrderOptions), orderBy);
                }

                List<RecipeComment> entities = await _commentRepos.GetRecipeComments(recipeId, languageId, orderOptions);

                List<CommentResponse> tree = new List<CommentResponse>();
                foreach (RecipeComment entity in entities.Where(e => !e.ParentCommentId.HasValue))
                {
                    CommentResponse root = new CommentResponse(entity);
                    AddReplies(entities, root);

                    tree.Add(root);
                }

                return Ok(tree);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("api/recipes/{recipeId}/comments")]
        [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CommentResponse>> CreateRecipeComment(int recipeId, CommentRequest body)
        {
            try
            {
                int languageId = Request.Headers.GetLanguageId();

                Guid currentUser = Request.Headers.GetAzureId();
                User user = await _userRepos.GetUserByAzureIdAsync(currentUser);
                if (user == null)
                {
                    throw new Exception("Could not find user");
                }

                List<RecipeCommentTranslation> translations = new List<RecipeCommentTranslation>
                {
                    new RecipeCommentTranslation()
                    {
                        IsOriginal = true,
                        Text = body.Text,
                        LanguageId = languageId // TODO: Auto-detect posted language
                    }
                };
                // translations.AddRange(_translationService.Get(translation)); // TODO: Call auto-translate service

                RecipeComment createdEntity = await _commentRepos.CreateRecipeComment(user, recipeId, body.ParentCommentId, translations);
                return Ok(new CommentResponse(createdEntity));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <param name="id">DateCreatedAsc, DateCreatedDesc</param>
        [HttpDelete]
        [Route("api/recipes/comment/{id}")]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CommentResponse>> CreateRecipeComment(int id)
        {
            try
            {
                Guid currentUser = Request.Headers.GetAzureId();
                User user = await _userRepos.GetUserByAzureIdAsync(currentUser);
                if (user == null)
                {
                    throw new Exception("Could not find user");
                }
                await _commentRepos.DeleteComment(id, user.Id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private void AddReplies(List<RecipeComment> entities, CommentResponse root)
        {
            IEnumerable<RecipeComment> repliesData = entities.Where(e => e.ParentCommentId.HasValue && e.ParentCommentId.Value == root.Id);
            foreach (RecipeComment replyData in repliesData)
            {
                CommentResponse reply = new CommentResponse(replyData);
                AddReplies(entities, reply);

                root.Replies.Add(reply);
            }
        }
    }

}

namespace WorldsBelly.API.Responses
{
    public class CommentResponse
    {
        public int Id { get; set; }
        public UserView CreatedByUser { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public CommentTranslationResponse OriginalText { get; set; }
        public CommentTranslationResponse TranslatedText { get; set; }
        public List<CommentResponse> Replies { get; set; }
        public bool HasMoreReplies { get; set; }

        public CommentResponse()
        {
            Replies = new List<CommentResponse>();
        }

        public CommentResponse(RecipeComment entity) : this()
        {
            Id = entity.Id;
            CreatedAt = entity.CreatedAt;
            CreatedByUser = new UserView()
            {
                Id = (entity.DeletedAt != null) ? 0 : entity.CreatedByUser.Id,
                //ADObjectId = entity.CreatedByUser.ADObjectId,
                Username = (entity.DeletedAt != null) ? "Deleted Comment" : entity.CreatedByUser.Username,
                Image = (entity.DeletedAt != null) ? "" : entity.CreatedByUser.Image,
            };
            HasMoreReplies = false;

            var originalTranslation = entity.Translations.FirstOrDefault(t => t.IsOriginal);
            OriginalText = new CommentTranslationResponse()
            {
                LanguageId = originalTranslation.LanguageId,
                Text = originalTranslation.Text
            };

            var translation = entity.Translations.FirstOrDefault(t => !t.IsOriginal);
            if (translation != null)
            {
                TranslatedText = new CommentTranslationResponse()
                {
                    LanguageId = translation.LanguageId,
                    Text = translation.Text
                };
            }
            else
            {
                TranslatedText = new CommentTranslationResponse()
                {
                    LanguageId = originalTranslation.LanguageId,
                    Text = originalTranslation.Text
                };
            }
        }
    }

    public class CommentTranslationResponse
    {
        public int LanguageId { get; set; }
        public string Text { get; set; }
    }
}

namespace WorldsBelly.API.Requests
{
    public class CommentRequest
    {
        public int? ParentCommentId { get; set; }
        public string Text { get; set; }
    }
}