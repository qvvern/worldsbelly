using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.API.Models;
using WorldsBelly.API.Services.Interfaces;
using WorldsBelly.API.Utilities.Mappers;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Repository.Interfaces;
using WorldsBelly.DataAccess.Utilities.Extensions;

namespace WorldsBelly.API.Services
{
    public class IngredientService : IIngredientService
    {
        private IIngredientRepository _ingredientRepository;

        public IngredientService(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        public async Task<ActionResult<ICollection<IngredientView>>> GetIngredientsAsync(int? startAt, int? amount, string search)
        {
            var response = _ingredientRepository.GetIngredientTranslationsAsync(startAt, amount, search);
            return response.Select(ResponseMapper.Map).ToList();
        }

        public async Task<ActionResult<ICollection<IngredientView>>> GetIngredientsByIdAsync(List<int> ids)
        {
            var response = _ingredientRepository.GetIngredientsByIdAsync(ids);
            return response.Select(ResponseMapper.Map).ToList();
        }

    }
}
