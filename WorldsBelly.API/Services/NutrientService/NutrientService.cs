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
    public class NutrientService : INutrientService
    {
        private INutrientRepository _nutrientRepository;

        public NutrientService(INutrientRepository nutrientRepository)
        {
            _nutrientRepository = nutrientRepository;
        }

        public async Task<ActionResult<ICollection<NutrientView>>> GetNutrientsAsync()
        {
            var response = await _nutrientRepository.GetNutrientsAsync().ConfigureAwait(false);
            return response.Select(ResponseMapper.Map).ToList();
        }

    }
}
