using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorldsBelly.API.Models;

namespace WorldsBelly.API.Services.Interfaces
{
    public interface IIngredientService
    {
        Task<ActionResult<ICollection<IngredientView>>> GetIngredientsAsync(int? startAt, int? amount, string search);
        Task<ActionResult<ICollection<IngredientView>>> GetIngredientsByIdAsync(List<int> ids);
    }
}
