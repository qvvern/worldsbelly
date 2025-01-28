using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Contexts;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Repository.Interfaces;
using WorldsBelly.DataAccess.Services;
using WorldsBelly.DataAccess.Services.Interfaces;
using WorldsBelly.DataAccess.Utilities;

namespace WorldsBelly.DataAccess.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _db;
        private readonly INotificationRepository _notificationRepository;

        public CommentRepository(AppDbContext dbContext, INotificationRepository notificationRepository)
        {
            _db = dbContext;
            _notificationRepository = notificationRepository;
        }

        public async Task<RecipeComment> GetComment(int commentId)
        {
            return await _db.RecipeComments
                .Include(c => c.Translations)
                .Include(c => c.CreatedByUser)
                .Include(c => c.ModifiedByUser)
                .FirstOrDefaultAsync(c => c.Id == commentId);
        }

        public async Task<List<RecipeComment>> GetRecipeComments(int recipeId, int languageId, QueryOrderOptions? orderBy = null)
        {
            IQueryable<RecipeComment> comments = _db.RecipeComments
                .Where(c => c.RecipeId == recipeId)
                .Include(c => c.CreatedByUser)
                .Include(c => c.Translations)
                .ThenInclude(t => t.Language)
                .Where(t => t.Translations.Any(t => t.IsOriginal || t.LanguageId == languageId));

            // Order list by sort order
            if (orderBy != null && orderBy == QueryOrderOptions.DateCreatedDesc)
            {
                comments = comments.OrderByDescending(c => c.CreatedAt);
            }
            else // Default to oldest first
            {
                comments = comments.OrderBy(c => c.CreatedAt);
            }

            return await comments.ToListAsync();
        }

        public async Task<RecipeComment> CreateRecipeComment(User createdBy, int recipeId, int? parentCommentId, List<RecipeCommentTranslation> translations)
        {
            try
            {
                var languageId = translations.FirstOrDefault().LanguageId;
                Recipe recipe = await _db.Recipes.Include(_ => _.Translations.Where(_ => _.LanguageId == languageId)).FirstOrDefaultAsync(r => r.Id == recipeId);
                if (recipe == null)
                {
                    throw new Exception("Could not find recipe");
                }

                RecipeComment entity = new RecipeComment();
                entity.RecipeId = recipeId;
                entity.Translations = translations;
                entity.CreatedByUserId = createdBy.Id;

                if (parentCommentId.HasValue)
                {
                    RecipeComment parentComment = await _db.RecipeComments.FirstOrDefaultAsync(c => c.Id == parentCommentId.Value);
                    if (parentComment == null)
                    {
                        throw new Exception("Could not find parent comment");
                    }
                    else if(parentComment.CreatedByUserId == createdBy.Id)
                    {
                        throw new Exception("You cannot reply to your own comment.");
                    }

                    entity.ParentCommentId = parentComment.Id;
                    entity.Level = parentComment.Level++;
                    entity.RootId = parentComment.RootId;
                }
                else
                {
                    entity.Level = 1;
                    entity.RootId = entity.Id;
                }
                _db.RecipeComments.Add(entity);
                await _db.SaveChangesAsync();

                recipe.CalculatedTotalComments = recipe.CalculatedTotalComments + 1;
                _db.Recipes.Update(recipe);
                await _db.SaveChangesAsync();

                var createdComment = await GetComment(entity.Id);

                if(createdComment.Level == 1)
                {
                    // notify owner about new comment
                    await _notificationRepository.CreateNotificationAsync(new Notification()
                    {
                        TemplateId = 1,
                        ReceiverId = recipe.CreatedByUserId,
                        SenderId = createdComment.CreatedByUserId,
                        ActionUrl = $"/en/recipes/{recipe.Translations.FirstOrDefault().Id}?comments={createdComment.Id}"
                    });
                }
                return createdComment;
            }
            catch(Exception e)
            {
                throw new Exception(e.InnerException.Message);
            }
        }

        public async Task DeleteComment(int commentId, int deletedByUserId)
        {
            User user = await GetUser(deletedByUserId);
            RecipeComment comment = await GetComment(commentId);
            if(user.Id != comment.CreatedByUserId)
            {
                throw new Exception("Only the user who made the comment can delete it");
            }
            RecipeComment recipeComment = await _db.RecipeComments.AsNoTracking().FirstOrDefaultAsync(r => r.Id == comment.Id);
            Recipe recipe = await _db.Recipes.FirstOrDefaultAsync(r => r.Id == recipeComment.RecipeId);
            recipe.CalculatedTotalComments = recipe.CalculatedTotalComments - 1;
            if (recipe.CalculatedTotalComments < 0) recipe.CalculatedTotalComments = 0;
            comment.DeletedAt = DateTimeOffset.Now;
            comment.DeletedByUser = user;

            if (_db.RecipeComments.Any(c => c.ParentCommentId.HasValue && c.ParentCommentId.Value == comment.Id))
            {
                foreach (RecipeCommentTranslation translation in comment.Translations)
                {
                    translation.Text = null;
                }
            }
            else
            {
                _db.RecipeComments.Remove(comment);
            }

            _db.Recipes.Update(recipe);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateComment(int commentId, int modifiedByUserId, List<RecipeCommentTranslation> translations)
        {
            User user = await GetUser(modifiedByUserId);
            RecipeComment comment = await GetComment(commentId);

            comment.Translations = translations;
            comment.ModifiedAt = DateTimeOffset.Now;
            comment.ModifiedByUser = user;

            await _db.SaveChangesAsync();
        }

        public async Task UpdateRootId(int commentId, int rootId)
        {
            RecipeComment comment = await GetComment(commentId);
            comment.RootId = rootId;
            await _db.SaveChangesAsync();
        }

        private async Task<User> GetUser(int userId)
        {
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception("Could not find user");
            }

            return user;
        }
    }
}
