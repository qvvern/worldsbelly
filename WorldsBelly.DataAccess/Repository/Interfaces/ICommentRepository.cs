using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Utilities;

namespace WorldsBelly.DataAccess.Repository.Interfaces
{
    public interface ICommentRepository
    {
        Task<RecipeComment> GetComment(int commentId);

        Task<List<RecipeComment>> GetRecipeComments(int recipeId, int languageId, QueryOrderOptions? orderBy = null);

        Task<RecipeComment> CreateRecipeComment(User createdBy, int recipeId, int? parentCommentId, List<RecipeCommentTranslation> translations);

        /// <summary>
        /// Will delete the comment if no replies, or remove the text if there are replies.
        /// </summary>
        /// <param name="commentId">The comment to delete</param>
        Task DeleteComment(int commentId, int deletedByUserId);

        Task UpdateComment(int commentId, int modifiedByUserId, List<RecipeCommentTranslation> translations);
    }
}
