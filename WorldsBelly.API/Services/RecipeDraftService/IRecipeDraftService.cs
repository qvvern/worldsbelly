using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorldsBelly.API.Models;
using WorldsBelly.DataAccess.Entities;

namespace WorldsBelly.API.Services.Interfaces
{
    public interface IRecipeDraftService
    {
        Task<ActionResult<ICollection<RecipeView>>> GetRecipeDraftsAsync();
        Task<ActionResult<RecipeView>> GetRecipeDraftAsync(Guid id);
        Task<RecipeView> UpdateRecipeDraftAsync(Guid id, RecipeView recipeDraft);
        Task PublishRecipeDraftAsync(Guid id);
        Task<ActionResult<RecipeView>> CreateRecipeDraftTranslationAsync(CreateRecipeDraftView recipeDraft);
        Task<ActionResult> DeleteRecipeDraftTranslationAsync(Guid id);
    }
}
